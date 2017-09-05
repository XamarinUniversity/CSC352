using System.Threading.Tasks;
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

			firstSensor = left;
			secondSensor = right;
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

		static private object guard = new object();
		void Eat()
		{
			Monitor.Enter(guard);

			if (firstSensor.StepOnSensor(this))
			{
				if (secondSensor.StepOnSensor(this))
				{
					TakeABite();
					secondSensor.LeaveBowl(this);
				}
				firstSensor.LeaveBowl(this);
			}

			Monitor.Exit(guard);
		}

		void TakeABite ()
		{
			CurrentState = State.Eating;
			Task.Delay(50).Wait();
			Bites++;
		}
	}
}