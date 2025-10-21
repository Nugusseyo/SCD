using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Work.JYG.Code.Chessboard.Pieces;

[CustomEditor(typeof(ObjectVectorListSO))]
public class ChessSOEditor : UnityEditor.Editor
{
    [SerializeField] private VisualTreeAsset vectorListView = default;

    public override VisualElement CreateInspectorGUI()
    {
        const int WIDTH = 3;
        const int HEIGHT = 3;
        
        VisualElement root = new VisualElement();
        InspectorElement.FillDefaultInspector(root, serializedObject, this);
        vectorListView.CloneTree(root);

        root.Q<Button>("SaveButton").clicked += HandleSaveBtnClick;
        root.Q<Button>("RookBtn").clicked += HandleRookBtnClick;
        root.Q<Button>("BishopBtn").clicked += HandleBishopBtnClick ;

        StringBuilder sb = new StringBuilder();

        for (int i = -HEIGHT; i <= HEIGHT; i++)
        {
            for (int j = -WIDTH; j <= WIDTH; j++)
            {
                if (j == 0 && i == 0)
                {
                    Button cBtn = root.Q<Button>("Center");
                    cBtn.style.backgroundColor = Color.magenta;
                    cBtn.clicked += () =>
                    {
                        ObjectVectorListSO list = target as ObjectVectorListSO;
                        list.VectorList.Clear();
                    };
                    continue;
                }

                sb.Clear();
                if (j < 0)
                {
                    sb.Append($"{j}");
                }
                else
                {
                    sb.Append($"0{j}");
                }

                if (i < 0)
                {
                    sb.Append($"{i}");
                }
                else
                {
                    sb.Append($"0{i}");
                }
                Button btn = root.Q<Button>($"{sb}Btn");
                //여기서 Vector 추출하고 List와 비교해주기
                btn.clicked += () =>
                {
                    char[] index = btn.name.ToCharArray();
                    int x = index[0] == '-' ? (index[1] - '0') * -1 : index[1] - '0';
                    int y = index[2] == '-' ? (index[3] - '0') * -1 : index[3] -'0';
                    HandleTileBtnClick(x, y, btn);
                };
            }
        }
        
        
        return root;
    }

    private void HandleBishopBtnClick()
    {
        ObjectVectorListSO t = target as ObjectVectorListSO;
        for (int i = -8; i <= 8; i++)
        {
            if (i == 0) continue;
            t.VectorList.Add(new Vector3Int(i, i, 0));
        }
        for (int i = -8; i <= 8; i++)
        {
            if (i == 0) continue;
            t.VectorList.Add(new Vector3Int(i, -i, 0));
        }
    }

    private void HandleRookBtnClick()
    {
        ObjectVectorListSO t = target as ObjectVectorListSO;
        for (int i = -8; i <= 8; i++)
        {
            if (i == 0) continue;
            t.VectorList.Add(new Vector3Int(i, 0, 0));
        }
        for (int i = -8; i <= 8; i++)
        {
            if (i == 0) continue;
            t.VectorList.Add(new Vector3Int(0, i, 0));
        }
    }

    private void HandleTileBtnClick(int x, int y, Button btn)
    {
        ObjectVectorListSO t = target as ObjectVectorListSO;
        if (btn.style.backgroundColor == Color.cyan)
        {
            btn.style.backgroundColor = Color.white;
            t.VectorList.RemoveAll(v => v == new Vector3Int(x, y, 0));
        }
        else
        {
            btn.style.backgroundColor = Color.cyan;
            t.VectorList.Add(new Vector3Int(x, y));
        }
    }

    private void HandleSaveBtnClick()
    {
        ObjectVectorListSO so = target as ObjectVectorListSO;

        so.VectorList = so.VectorList.Distinct().ToList();
        Debug.Log("VectorList Save!");
    }
}
