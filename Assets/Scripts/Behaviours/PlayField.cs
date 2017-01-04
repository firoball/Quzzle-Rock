using UnityEngine;
using System.Collections;
using Assets.Scripts.Static;
using Assets.Scripts.Structs;
using Assets.Scripts.Classes;

namespace Assets.Scripts.Behaviours
{
    public class PlayField : MonoBehaviour
    {
        private static PlayField singleton = null;

        private DataField<GameObject> m_fieldTokens;
        private DataField<int> m_fieldTypes;

        public static bool areNeighbours(GameObject token1, GameObject token2)
        {
            DataFieldPosition position1;
            DataFieldPosition position2;
            if (singleton != null)
            {
                return singleton.areNeighbours(token1, token2, out position1, out position2);
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

            //TODO: make this in a nice way without hardcoded values
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

            bool neighbours = areNeighbours(token1, token2, out position1, out position2);

            if (neighbours)
            {
                SwapTester test2 = new SwapTester(m_fieldTypes);
                if (test2.IsValidSwap(position1, position2, 3))
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

                    //TEST
                    CombinationFinder test = new CombinationFinder(m_fieldTypes, true);
                    test.Find(3);

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

        private bool areNeighbours(GameObject token1, GameObject token2, out DataFieldPosition pos1, out DataFieldPosition pos2)
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


        private void Encapsulate(GameObject go, ref Bounds bounds)
        {
            MeshRenderer renderer = go.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                bounds.Encapsulate(renderer.bounds);
            }
        }

    }
}
