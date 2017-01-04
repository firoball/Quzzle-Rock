using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Static;
using Assets.Scripts.Structs;
using Assets.Scripts.Classes;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class PlayField : MonoBehaviour
    {
        private static PlayField singleton = null;

        private DataField<GameObject> m_fieldTokens;
        private DataField<int> m_fieldTypes;
        private DataField<int> m_fieldTypesOriginal;

        private const int c_minChainLength = 3;

        public static bool AreNeighbours(GameObject token1, GameObject token2)
        {
            DataFieldPosition position1;
            DataFieldPosition position2;
            if (singleton != null)
            {
                return singleton.AreNeighbours(token1, token2, out position1, out position2);
            }
            else
            {
                return false;
            }
        }

        public static bool SwapTokens(GameObject token1, GameObject token2)
        {
            if (singleton != null)
            {
                return singleton.SwapTokens_internal(token1, token2);
            }
            else
            {
                return false;
            }
        }

        public static Bounds GetDimension()
        {
            if (singleton != null)
            {
                return singleton.GetDimension_internal();
            }
            else
            {
                return new Bounds(Vector3.zero, Vector3.one);
            }
        }

        void Awake()
        {
            if (singleton == null)
            {
                m_fieldTokens = new DataField<GameObject>(Preferences.ColumnCount, Preferences.RowCount);
                m_fieldTypes = new DataField<int>(Preferences.ColumnCount, Preferences.RowCount);
                Populate();
                m_fieldTypesOriginal = m_fieldTypes.Copy();
                SpawnTokens();

                singleton = this;
            }
            else
            {
                Debug.LogWarning("PlayField: Multiple instances detected. Destroying...");
                Destroy(this);
            }
        }

        private void Populate()
        {
            System.Random rand = new System.Random();

            //populate
            DataFieldSize size = m_fieldTokens.Size();
            for (int column = 0; column < size.Columns; column++)
            {
                for (int row = 0; row < size.Rows; row++)
                {
                    int id = rand.Next(0, Preferences.TokenCount);
                    DataFieldPosition fieldPos = new DataFieldPosition(column, row);
                    m_fieldTypes.SetAtPosition(fieldPos, id);
                }
            }

            //eliminate any generated combinations
            CombinationFinder cf = new CombinationFinder(m_fieldTypes, true);
            int iterations = 1;

            //try maximum 100 iterations
            while (cf.Find(c_minChainLength) && (iterations < 100))
            {
                List<Combination> comboList = cf.Combinations;
                foreach (Combination combination in comboList)
                {
                    //only replace id if more than one id is available (this should always be the case)
                    if ((combination.Positions.Count > 1) && (Preferences.TokenCount > 1))
                    {
                        int id;
                        do
                        {
                            id = rand.Next(0, Preferences.TokenCount);
                        } while (id == combination.Id);
                        DataFieldPosition fieldPos = combination.Positions[1];
                        m_fieldTypes.SetAtPosition(fieldPos, id);
                    }
                }
                cf.Clear();
                iterations++;
            }

            Debug.Log("Populate: " + iterations + " iterations performed.");
        }

        private void SpawnTokens()
        {
            //TODO: make this in a nice way without hardcoded values
            DataFieldSize size = m_fieldTokens.Size();
            float xWidth = size.Columns * 1.5f - 1.5f;
            float yWidth = size.Rows * 1.5f - 1.5f;

            float x = -0.5f * xWidth;
            for (int column = 0; column < size.Columns; column++)
            {
                float y = -0.5f * yWidth;
                for (int row = 0; row < size.Rows; row++)
                {
                    DataFieldPosition fieldPos = new DataFieldPosition(column, row);
                    int id = m_fieldTypes.GetFromPosition(fieldPos);
                    GameObject go = TokenConfig.StandardToken[id];
                    Vector3 position = new Vector3(x, y, 0.0f);
                    GameObject token = (GameObject)Instantiate(go, position, Quaternion.identity);
                    token.name = "Token" + column + "_" + row;
                    m_fieldTokens.SetAtPosition(fieldPos, token);
                    y += 1.5f;
                }
                x += 1.5f;
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

                    //TEST REMOVE (playfield will be out of sync)
                    CombinationFinder cf = new CombinationFinder(m_fieldTypes, true);
                    if (cf.Find(c_minChainLength))
                    {
                        //TODO: move to TokenReplacer + add Replacement functionality
                        List<Combination> comboList = cf.Combinations;
                        foreach (Combination combination in comboList)
                        {
                            foreach (DataFieldPosition pos in combination.Positions)
                            {
                                GameObject go = m_fieldTokens.GetFromPosition(pos);
                                ExecuteEvents.Execute<ITokenEventTarget>(go, null, (x, y) => x.OnRemove());
                                m_fieldTokens.SetAtPosition(pos, null);
                            }
                        }
                    }

                    allowSwap = true;
                }
            }

            return allowSwap;
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


        private void Encapsulate(GameObject gameObject, ref Bounds bounds)
        {
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                bounds.Encapsulate(renderer.bounds);
            }
        }

    }
}
