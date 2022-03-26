using System;
using System.Collections.Generic;
using System.Text;
using LZLUnityTool.Plugins.CommonPlugin.Manager;
using LZLUnityTool.Plugins.InventoryPlugin.System.TaskSystem.Scripts.SO.Charactor;

namespace LZLUnityTool.Plugins.InvntoryPlugin.System.TaskSystem.Scripts.Base.Tools
{
public static class MiscFuntion
{
    
    public static bool CheckCondition(ConditionGroup group)
    {
        if (!group) return true;
        bool calFailed = false;
        if (string.IsNullOrEmpty(group.Relational)) return group.Conditions.TrueForAll(x => CheckCondition(x));
        if (group.Conditions.Count < 1) calFailed = true;
        else
        {
            var cr = group.Relational.Replace(" ", "").ToCharArray();//删除所有空格才开始计算
            List<string> RPN = new List<string>();//逆波兰表达式
            string indexStr = string.Empty;//数字串
            Stack<char> optStack = new Stack<char>();//运算符栈
            for (int i = 0; i < cr.Length; i++)
            {
                char c = cr[i];
                string item;
                if (c < '0' || c > '9')
                {
                    if (!string.IsNullOrEmpty(indexStr))
                    {
                        item = indexStr;
                        indexStr = string.Empty;
                        GetRPNItem(item);
                    }
                    if (c == '(' || c == ')' || c == '+' || c == '*' || c == '~')
                    {
                        item = c + "";
                        GetRPNItem(item);
                    }
                    else
                    {
                        calFailed = true;
                        break;
                    }//既不是数字也不是运算符，直接放弃计算
                }
                else
                {
                    indexStr += c;//拼接数字
                    if (i + 1 >= cr.Length)
                    {
                        item = indexStr;
                        indexStr = string.Empty;
                        GetRPNItem(item);
                    }
                }
            }
            while (optStack.Count > 0)
                RPN.Add(optStack.Pop() + "");
            Stack<bool> values = new Stack<bool>();
            foreach (var item in RPN)
            {
                //Debug.Log(item);
                if (int.TryParse(item, out int index))
                {
                    if (index >= 0 && index < group.Conditions.Count)
                        values.Push(CheckCondition(group.Conditions[index]));
                    else
                    {
                        //Debug.Log("return 1");
                        return true;
                    }
                }
                else if (values.Count > 1)
                {
                    if (item == "+") values.Push(values.Pop() | values.Pop());
                    else if (item == "~") values.Push(!values.Pop());
                    else if (item == "*") values.Push(values.Pop() & values.Pop());
                }
                else if (item == "~") values.Push(!values.Pop());
            }
            if (values.Count == 1)
            {
                //Debug.Log("return 2");
                return values.Pop();
            }

            void GetRPNItem(string item)
            {
                //Debug.Log(item);
                if (item == "+" || item == "*" || item == "~")//遇到运算符
                {
                    char opt = item[0];
                    if (optStack.Count < 1) optStack.Push(opt);//栈空则直接入栈
                    else while (optStack.Count > 0)//栈不空则出栈所有优先级大于或等于opt的运算符后才入栈opt
                        {
                            char top = optStack.Peek();
                            if (top + "" == item || top == '~' || top == '*' && opt == '+')
                            {
                                RPN.Add(optStack.Pop() + "");
                                if (optStack.Count < 1)
                                {
                                    optStack.Push(opt);
                                    break;
                                }
                            }
                            else
                            {
                                optStack.Push(opt);
                                break;
                            }
                        }
                }
                else if (item == "(") optStack.Push('(');
                else if (item == ")")
                {
                    while (optStack.Count > 0)
                    {
                        char opt = optStack.Pop();
                        if (opt == '(') break;
                        else RPN.Add(opt + "");
                    }
                }
                else if (int.TryParse(item, out _)) RPN.Add(item);//遇到数字
            }
        }
        if (!calFailed)
        {
            //Debug.Log("return 3");
            return true;
        }
        else
        {
            foreach (Condition con in group.Conditions)
                if (!CheckCondition(con))
                {
                    //Debug.Log("return 4");
                    return false;
                }
            //Debug.Log("return 5");
            return true;
        }
    }
    private static bool CheckCondition(Condition condition)
    {
        switch (condition.Type)
        {
            case ConditionType.CompleteQuest:
                QuestData quest = QuestManager.Instance.FindQuest(condition.RelatedQuest.ID);
                return quest && TimeManager.Instance.Days - quest.latestHandleDays >= condition.IntValue && quest.IsFinished;
            case ConditionType.AcceptQuest:
                quest = QuestManager.Instance.FindQuest(condition.RelatedQuest.ID);
                return quest && TimeManager.Instance.Days - quest.latestHandleDays >= condition.IntValue && quest.InProgress;
            case ConditionType.HasItem:
                //需要实现判断是否拥有某物品的方法
                //BackpackManager.Instance.HasItemWithID(condition.RelatedItem.ID);
                return true;
            case ConditionType.Level:
                switch (condition.CompareType)
                {
                    case ValueCompareType.Equals:
                        //需要判断角色等级是否等于condition.IntValue
                        return true;
                    case ValueCompareType.LargeThen:
                        //需要判断角色等级是否大于condition.IntValue
                        return true;
                    case ValueCompareType.LessThen:
                        //需要判断角色等级是否小于condition.IntValue
                        return true;
                    default:
                        return true;
                }
            default: return true;
        }
    }
}
}