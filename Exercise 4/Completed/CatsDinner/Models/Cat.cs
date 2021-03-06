﻿using System.Threading.Tasks;
using System.Threading;

namespace CatsDinner.Models
{
	public class Cat 
	{
		readonly Sensor firstSensor;
		readonly Sensor secondSensor;

		public enum State 
		{ 
			Sleeping, 
			Hungry, 
			Eating 
		};

		public string Name { get; private set; }
		public int Bites { get; private set; }
		public State CurrentState { get; private set; }

		public Cat(string name, Sensor left, Sensor right)
		{
			Name = name;
			CurrentState = State.Sleeping;

			firstSensor = (left.Id > right.Id) ? left : right;
			secondSensor = (left.Id > right.Id) ? right : left;
		}

		public void Run()
		{
			while (true) 
			{
				TakeNap();
				Eat();
			}
		}

		void TakeNap()
		{
			CurrentState = State.Sleeping;
			Task.Delay(50).Wait();
			CurrentState = State.Hungry;
		}

		void Eat()
		{
			lock (firstSensor)
			{
				if (firstSensor.StepOnSensor(this))
				{
					lock (secondSensor)
					{
						if (secondSensor.StepOnSensor(this))
						{
							TakeABite();
							secondSensor.LeaveBowl(this);
						}
					}
					firstSensor.LeaveBowl(this);
				}
			}
		}

		void TakeABite ()
		{
			CurrentState = State.Eating;
			Task.Delay(50).Wait();
			Bites++;
		}
	}
}