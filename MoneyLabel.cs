using Godot;
using System;
using System.Text;

public class MoneyLabel : Label
{
	public override void _Ready()
	{
		GameData.ConfigSaveEvent += OnConfigChanged;
		Text = AddCommasToNumber(GameData.Money.ToString());
	}

	private void OnConfigChanged(object sender, ConfigChangeEventArgs eventArgs)
	{
		if (eventArgs.ConfigName == "money")
			Text = AddCommasToNumber(eventArgs.Config.ToString());
	}

	private string AddCommasToNumber(string input)
	{
		StringBuilder stringBuilder = new StringBuilder(input);
		int commasAdded = 0;
		for (int index = 0; index < stringBuilder.Length; index++)
		{
			if (index % 3 == 0 && index != 0)
			{
				stringBuilder.Insert(stringBuilder.Length - (index + commasAdded), ",");
				commasAdded++;
			}
		}
		return $"£{stringBuilder}";
	}
}
