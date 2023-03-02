using System;
using System.Linq;
using SHAutomation.Core.Conditions;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Converters
{
    public static class ConditionConverter
    {
        public static UIA.IUIAutomationCondition ToNative(UIA3Automation automation, ConditionBase condition)
        {
            switch (condition)
            {
                case PropertyCondition propCond:
                    return automation.NativeAutomation.CreatePropertyConditionEx(propCond.Property.Id, ValueConverter.ToNative(propCond.Value), (UIA.PropertyConditionFlags)propCond.PropertyConditionFlags);

                case BoolCondition boolCond:
                    return boolCond.BooleanValue ? automation.NativeAutomation.CreateTrueCondition() : automation.NativeAutomation.CreateFalseCondition();

                case NotCondition notCond:
                    var notNative = ToNative(automation, notCond.Condition);
                    return automation.NativeAutomation.CreateNotCondition(notNative);

                case AndCondition andCond:
                    if (andCond.ChildCount == 0)
                    {
                        return automation.NativeAutomation.CreateTrueCondition();
                    }
                    if (andCond.ChildCount == 1)
                    {
                        return ToNative(automation, andCond.Conditions[0]);
                    }
                    var andNative = andCond.Conditions.Select(c => ToNative(automation, c)).ToArray();
                    return automation.NativeAutomation.CreateAndConditionFromArray(andNative);

                case OrCondition orCond:
                    if (orCond.ChildCount == 0)
                    {
                        return automation.NativeAutomation.CreateFalseCondition();
                    }
                    if (orCond.ChildCount == 1)
                    {
                        return ToNative(automation, orCond.Conditions[0]);
                    }
                    var orNative = orCond.Conditions.Select(c => ToNative(automation, c)).ToArray();
                    return automation.NativeAutomation.CreateOrConditionFromArray(orNative);

                default:
                    throw new ArgumentException("Unknown condition type");
            }
        }
    }
}
