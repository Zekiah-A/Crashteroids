using Godot;
using System;
using System.Text;

public static class Utils
{
    public static string AddCommasToNumbers(int inputNumber)
    {
        StringBuilder stringBuilder = new StringBuilder(inputNumber);
        int commasAdded = 0;
        for (int index = 0; index < stringBuilder.Length; index++)
        {
            if (index % 3 == 0 && index != 0)
            {
                stringBuilder.Insert(stringBuilder.Length - (index + commasAdded), ",");
                commasAdded++;
            }
        }

        return stringBuilder.ToString();
    }
}
