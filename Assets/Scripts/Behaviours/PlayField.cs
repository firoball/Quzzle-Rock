using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Assets.Scripts.Structs;
using Assets.Scripts.Classes;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class PlayField : MonoBehaviour
    {
        private static PlayField s_singleton = null;

        [SerializeField]
        [Range(10, 1000)]
        private int m_stackSize = 100;
        [SerializeField]
        private bool m_debug = false;

        private DataField<GameObject> m_fieldTokens;
        private DataField<int> m_fieldTypes;
        private TokenStack m_tokenStack;

        private const int c_minChainLength = 3;
        private const float c_worldSize = 1.3f;
        private const float c_ClearDelay = 0.07f;
        private const int c_maxIterations = 100;

        #region static functions

        public static bool AreNeighbours(GameObject token1, GameObject token2)
        {
            DataFieldPosition position1;
            DataFieldPosition position2;
            if (s_singleton != null)
            {
                return s_singleton.AreNeighbours(token1, token2, out position1, out position2);
            }
            else
            {
                return false;
            }
        }

        public static bool SwapTokens(GameObject token1, GameObject token2)
        {
            if (s_singleton != null)
            {
                return s_singleton.SwapTokens_internal(token1, token2);
            }
            else
            {
                return false;
            }
        }

        public static bool ResolveCombinations(out List<Combination> comboList)
        {
            if (s_singleton != null)
            {
                return s_singleton.ResolveCombinations_internal(out comboList);
            }
            else
            {
                comboList = null;
                return false;
            }

        }

        public static Bounds GetDimension()
        {
            if (s_singleton != null)
            {
                return s_singleton.GetDimension_internal();
            }
            else
            {
                return new Bounds(Vector3.zero, Vector3.one);
            }
        }

        public static Vector3 GetCombinationCenter(Combination combination)
        {
            if (s_singleton != null)
            {
                return s_singleton.GetCombinationCenter_internal(combination);
            }
            else
            {
                return Vector3.zero;
            }
        }

        public static bool IsStuck()
        {
            if (s_singleton != null)
            {
                return s_singleton.IsStuck_internal();
            }
            else
            {
                return true;
            }
        }

        public static bool Refill()
        {
            if (s_singleton != null)
            {
                return s_singleton.Refill_internal();
            }
            else
            {
                return true;
            }
        }

        public static void Populate()
        {
            if (s_singleton != null)
            {
                s_singleton.Populate_internal();
                s_singleton.SpawnTokens();
            }
        }

        public static float Clear()
        {
            if (s_singleton != null)
            {
                return s_singleton.Clear(false);
            }
            else
            {
                return 0.0f;
            }
        }

        public static void Restart(bool newStack)
        {
            if (s_singleton != null)
            {
                s_singleton.Clear(true);
                if (newStack)
                {
                    s_singleton.m_tokenStack = new TokenStack(s_singleton.m_stackSize, s_singleton.m_debug);
                }
                else
                {
                    s_singleton.m_tokenStack.Restart();
                }
                Populate();
            }
        }

        public static void CreateTokensFromDictionary(Dictionary<DataFieldPosition, int> tokenDict)
        {
            if (s_singleton != null)
            {
                s_singleton.CreateTokensFromDictionary_internal(tokenDict);
            }
        }

        public static void Lock()
        {
            Token.Locked = true;
        }

        public static void Unlock()
        {
            Token.Locked = false;
        }

        #endregion

        #region unity callbacks

        void Awake()
        {
            if (s_singleton == null)
            {
                DataFieldSize size = new DataFieldSize(Preferences.Current.ColumnCount, Preferences.Current.RowCount);
                m_fieldTokens = new DataField<GameObject>(size);
                m_fieldTypes = new DataField<int>(size);
                m_tokenStack = new TokenStack(m_stackSize, m_debug);
                Populate_internal();
                SpawnTokens();

                s_singleton = this;
            }
            else
            {
                Debug.Log("PlayField: Multiple instances detected. Destroying...");
                Destroy(this);
            }
        }

        #endregion

        #region private functions

        private void Populate_internal()
        {
            int populations = 0;
            //repeat until field is not stuck (still allows any valid token swap)
            do
            {
                populations++;
                //populate
                DataFieldSize size = m_fieldTokens.Size();
                for (int column = 0; column < size.Columns; column++)
                {
                    for (int row = 0; row < size.Rows; row++)
                    {
                        int id = m_tokenStack.GetNextTokenId(column);
                        DataFieldPosition fieldPos = new DataFieldPosition(column, row);
                        m_fieldTypes.SetAtPosition(fieldPos, id);
                    }
                }

                //eliminate any generated combinations
                CombinationFinder cf = new CombinationFinder(m_fieldTypes, m_debug);
                int iterations = 1;

                //try maximum 100 iterations
                while (cf.Find(c_minChainLength) && (iterations < c_maxIterations))
                {
                    List<Combination> comboList = cf.Combinations;
                    foreach (Combination combination in comboList)
                    {
                        //only replace id if more than one id is configured (this should always be the case)
                        if ((combination.Positions.Count > 1) && (Preferences.Current.TokenCount > 1))
                        {
                            int id;
                            do
                            {
                                id = m_tokenStack.GetNextTokenId(combination.Positions[1].Column);
                            } while (id == combination.Id);
                            DataFieldPosition fieldPos = combination.Positions[1];
                            m_fieldTypes.SetAtPosition(fieldPos, id);
                        }
                    }
                    cf.Clear();
                    iterations++;
                }

                if (m_debug)
                {
                    Debug.Log("Populate: " + populations + "x" + iterations + " iterations performed.");
                }
            } while (IsStuck_internal() && (populations < c_maxIterations));
        }

        private void SpawnTokens()
        {
            DataFieldSize size = m_fieldTokens.Size();
            for (int column = 0; column < size.Columns; column++)
            {
                for (int row = 0; row < size.Rows; row++)
                {
                    DataFieldPosition fieldPos = new DataFieldPosition(column, row);
                    int id = m_fieldTypes.GetFromPosition(fieldPos);
                    GameObject go = TokenConfig.StandardToken[id];
                    Vector3 position = TransformFieldToWorld(fieldPos);

                    GameObject token = (GameObject)Instantiate(go, position, Quaternion.identity);
                    token.name = "Token" + column + "_" + row;
                    m_fieldTokens.SetAtPosition(fieldPos, token);
                }
            }

        }

        private bool SwapTokens_internal(GameObject token1, GameObject token2)
        {
            bool allowSwap = false;

            DataFieldPosition position1;
            DataFieldPosition position2;

            bool neighbours = AreNeighbours(token1, token2, out position1, out position2);

            if (neighbours)
            {
                SwapTester swapTester = new SwapTester(m_fieldTypes);
                if (swapTester.IsValidSwap(position1, position2, c_minChainLength))
                {
                    string name = token1.name;
                    token1.name = token2.name;
                    token2.name = name;
                    m_fieldTokens.SetAtPosition(position1, token2);
                    m_fieldTokens.SetAtPosition(position2, token1);

                    int id1 = m_fieldTypes.GetFromPosition(position1);
                    int id2 = m_fieldTypes.GetFromPosition(position2);
                    m_fieldTypes.SetAtPosition(position1, id2);
                    m_fieldTypes.SetAtPosition(position2, id1);

                    PlayTurn.PlayerDone();
                    allowSwap = true;
                    AudioManager.Play("valid swap");
                }
                else
                {
                    AudioManager.Play("invalid swap");
                }
            }

            return allowSwap;
        }

        private bool ResolveCombinations_internal(out List<Combination> comboList)
        {
            bool found = false;
            comboList = null;
            CombinationFinder cf = new CombinationFinder(m_fieldTypes, m_debug);
            if (cf.Find(c_minChainLength))
            {
                found = true;
                comboList = cf.Combinations;
                Dictionary<DataFieldPosition, int> newTokenDict = new Dictionary<DataFieldPosition, int>();
                foreach (Combination combination in comboList)
                {
                    int[] result;
                    bool replace = ReplacementConfig.Find(combination.Id, combination.Positions.Count, out result);
                    for (int i = 0; i < combination.Positions.Count; i++)
                    {
                        DataFieldPosition pos = combination.Positions[i];
                        //remove old token
                        GameObject oldToken = m_fieldTokens.GetFromPosition(pos);
                        ExecuteEvents.Execute<ITokenEventTarget>(oldToken, null, (x, y) => x.OnRemove());
                        m_fieldTokens.SetAtPosition(pos, null);
                        m_fieldTypes.SetAtPosition(pos, -1);

                        /* schedule replacement token to be created, if any. Check against result length
                         * since replacement might be shorter than actual combination (fallback).
                         * Avoid double creation of token at one position caused by layered combinations
                         */
                        if (replace && (i < result.Length) &&
                            (result[i] != -1) && !newTokenDict.ContainsKey(pos))
                        {
                            newTokenDict.Add(pos, result[i]);
                        }
                    }

                }

                //now create scheduled repalcement tokens
                CreateTokensFromDictionary_internal(newTokenDict);
                newTokenDict.Clear();
            }
            return found;
        }

        private Vector3 GetCombinationCenter_internal(Combination combination)
        {
            Vector3 center = Vector3.zero;
            foreach (DataFieldPosition position in combination.Positions)
            {
                center += TransformFieldToWorld(position);
            }
            if (combination.Positions.Count > 0)
            {
                center /= Convert.ToSingle(combination.Positions.Count);
            }
            return center;
        }

        private Bounds GetDimension_internal()
        {
            Bounds bounds = new Bounds(Vector3.zero, Vector3.one);

            DataFieldPosition pos;
            pos = new DataFieldPosition(m_fieldTokens.MinColumn, m_fieldTokens.MinRow);
            Encapsulate(m_fieldTokens.GetFromPosition(pos), ref bounds);
            pos = new DataFieldPosition(m_fieldTokens.MinColumn, m_fieldTokens.MaxRow);
            Encapsulate(m_fieldTokens.GetFromPosition(pos), ref bounds);
            pos = new DataFieldPosition(m_fieldTokens.MaxColumn, m_fieldTokens.MinRow);
            Encapsulate(m_fieldTokens.GetFromPosition(pos), ref bounds);
            pos = new DataFieldPosition(m_fieldTokens.MaxColumn, m_fieldTokens.MaxRow);
            Encapsulate(m_fieldTokens.GetFromPosition(pos), ref bounds);

            return bounds;
        }

        private bool IsStuck_internal()
        {
            bool canSwap = false;
            DataFieldPosition position1;
            DataFieldPosition position2;
            SwapTester swapTester = new SwapTester(m_fieldTypes);

            for (int row = m_fieldTypes.MinRow; row <= m_fieldTypes.MaxRow && !canSwap; row++)
            {
                for (int column = m_fieldTypes.MinColumn; column <= m_fieldTypes.MaxColumn && !canSwap; column++)
                {
                    /* stuck check is done by simply trying all possible swaps and abort once
                     * the first swap was successful
                     */
                    position1 = new DataFieldPosition(column, row);
                    if (column < m_fieldTypes.MaxColumn)
                    {
                        position2 = new DataFieldPosition(column + 1, row);
                        canSwap = swapTester.IsValidSwap(position1, position2, c_minChainLength);
                    }
                    if (row < m_fieldTypes.MaxRow && !canSwap)
                    {
                        position2 = new DataFieldPosition(column, row + 1);
                        canSwap = swapTester.IsValidSwap(position1, position2, c_minChainLength);
                    }
                }
            }
            return !canSwap;
        }

        private bool Refill_internal()
        {
            for (int row = m_fieldTypes.MinRow; row <= m_fieldTypes.MaxRow; row++)
            {
                for (int column = m_fieldTypes.MinColumn; column <= m_fieldTypes.MaxColumn; column++)
                {
                    DataFieldPosition upperPos = new DataFieldPosition(column, row + 1);
                    DataFieldPosition lowerPos = new DataFieldPosition(column, row);
                    int upperId;
                    int lowerId = m_fieldTypes.GetFromPosition(lowerPos);

                    //gap detected
                    if (lowerId == -1)
                    {
                        GameObject token;
                        if (row < m_fieldTypes.MaxRow)
                        {
                            //get id from upper token
                            upperId = m_fieldTypes.GetFromPosition(upperPos);
                        }
                        else
                        {
                            //already uppermsot row - take id from token stack
                            upperId = m_tokenStack.GetNextTokenId(column);
                        }

                        //found token in row above
                        if (upperId != -1)
                        {
                            //move identifier downwards
                            m_fieldTypes.SetAtPosition(lowerPos, upperId);
                            m_fieldTypes.SetAtPosition(upperPos, -1);
                            //move token downwards
                            if (row < m_fieldTypes.MaxRow)
                            {
                                //take token from upper row
                                token = m_fieldTokens.GetFromPosition(upperPos);
                            }
                            else
                            {
                                //id was taken from token stack - create new token
                                GameObject prefab = GetTokenPrefabForId(upperId);
                                Vector3 offsidePos = TransformFieldToWorld(upperPos);
                                token = (GameObject)Instantiate(prefab, offsidePos, Quaternion.identity);
                            }
                            token.name = "Token" + lowerPos.Column + "_" + lowerPos.Row;
                            m_fieldTokens.SetAtPosition(lowerPos, token);
                            m_fieldTokens.SetAtPosition(upperPos, null);
                            //trigger token animation
                            Vector3 worldPos = TransformFieldToWorld(lowerPos);
                            ExecuteEvents.Execute<ITokenEventTarget>(token, null,
                                (x, y) => x.OnMoveTo(worldPos));
                        }
                    }

                }
            }
            return !HasGaps();
        }

        private bool HasGaps()
        {
            bool gaps = false;
            for (int row = m_fieldTypes.MinRow; row <= m_fieldTypes.MaxRow && !gaps; row++)
            {
                for (int column = m_fieldTypes.MinColumn; column <= m_fieldTypes.MaxColumn && !gaps; column++)
                {
                    DataFieldPosition pos = new DataFieldPosition(column, row);
                    if (m_fieldTypes.GetFromPosition(pos) == -1)
                    {
                        gaps = true;
                    }
                }
            }
            return gaps;
        }

        private bool AreNeighbours(GameObject token1, GameObject token2, out DataFieldPosition pos1, out DataFieldPosition pos2)
        {
            DataFieldPosition position1;
            DataFieldPosition position2;
            bool neighbours = false;

            bool found1 = m_fieldTokens.Find(token1, out position1);
            bool found2 = m_fieldTokens.Find(token2, out position2);
            if (found1 && found2)
            {
                neighbours = position1.IsNeighbourOf(position2);
            }
            pos1 = position1;
            pos2 = position2;

            return neighbours;
        }

        private float Clear(bool hardClear)
        {
            float timer = 0.0f;
            for (int column = m_fieldTypes.MinColumn; column <= m_fieldTypes.MaxColumn; column++)
            {
                for (int row = m_fieldTypes.MinRow; row <= m_fieldTypes.MaxRow; row++)
                {
                    DataFieldPosition pos = new DataFieldPosition(column, row);
                    GameObject token = m_fieldTokens.GetFromPosition(pos);
                    if (hardClear)
                    {
                        Destroy(token);
                    }
                    else
                    {
                        ExecuteEvents.Execute<ITokenEventTarget>(token, null, (x, y) => x.OnRemove(timer));
                        timer += c_ClearDelay;
                    }
                }
            }
            m_fieldTokens.Clear();
            m_fieldTypes.Clear();

            return timer;
        }

        private void Encapsulate(GameObject obj, ref Bounds bounds)
        {
            if (obj != null)
            {
                MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }
        }

        private Vector3 TransformFieldToWorld(DataFieldPosition pos)
        {
            //this is not fastest, but it really does not matter at all...
            DataFieldSize size = m_fieldTokens.Size();
            float xWidth = (size.Columns - 1) * c_worldSize;
            float yWidth = (size.Rows - 1) * c_worldSize;
            float x = (-0.5f * xWidth) + (c_worldSize * pos.Column);
            float y = (-0.5f * yWidth) + (c_worldSize * pos.Row);
            return new Vector3(x, y, 0.0f);
        }

        private GameObject GetTokenPrefabForId(int id)
        {
            GameObject go = null;
            if (id > -1 && id < TokenConfig.StandardToken.Length)
            {
                go = TokenConfig.StandardToken[id];
            }
            else if (id > 99 && id < 100 + TokenConfig.SpecialToken.Length)
            {
                go = TokenConfig.SpecialToken[id - 100];
            }
            return go;
        }

        private void CreateTokensFromDictionary_internal(Dictionary<DataFieldPosition, int> tokenDict)
        {
            foreach (KeyValuePair<DataFieldPosition, int> entry in tokenDict)
            {
                int id = entry.Value;
                DataFieldPosition pos = entry.Key;

                GameObject oldToken = m_fieldTokens.GetFromPosition(pos);
                if (oldToken != null)
                {
                    Destroy(oldToken);
                }
                GameObject go = GetTokenPrefabForId(id);
                Vector3 worldPos = TransformFieldToWorld(pos);
                GameObject newToken = (GameObject)Instantiate(go, worldPos, Quaternion.identity);
                newToken.name = "Token" + pos.Column + "_" + pos.Row;
                m_fieldTokens.SetAtPosition(pos, newToken);
                m_fieldTypes.SetAtPosition(pos, id);
            }
        }
        #endregion
    }
}
