﻿using FluentAssertions;
using NUnit.Framework;

namespace HomeExercises
{
    public class ObjectComparison
    {
	    private Person expectedTsar;
	    private Person actualTsar;

	    [SetUp]
	    public void SetUp()
	    {
			// Так как и там и там одинаковые входные данные, решил перенести их в СетАп
			expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));

			actualTsar = TsarRegistry.GetCurrentTsar();
	    }

        [Test]
        [Description("Проверка текущего царя")]
        [Category("ToRefactor")]
        public void CheckCurrentTsar()
        {
	        actualTsar.ShouldBeEquivalentTo(expectedTsar,
	            config => config.Excluding(o => o.SelectedMemberInfo.Name == nameof(Person.Id)));

            // Используя такую проверку можно увидеть на каком именно поле провалился тест.
            // Также, если в классе появится новое поле, b в нем по-прежнему остается рекурсивная проверка
        }

        [Test]
        [Description("Альтернативное решение. Какие у него недостатки?")]
        public void CheckCurrentTsar_WithCustomEquality()
        {
            // Какие недостатки у такого подхода? 
            Assert.True(AreEqual(actualTsar, expectedTsar));

			// Если тест упадет, то будет непонятно, в каком именно поле будет ошибка
			// так как метод возвращает лишь ответ верны объекты или неверны
			
			// Также если будет слишком глубокая рекурсия родителей, то в теории может
			// произойти переполнение стэка

			// При появлении новых полей нужно переписывать функцию AreEqual

			// Функция AreEqual не защищена от ошибок реалзицаии, может произойти так что неправльно
			// написан сам тест а не решение
        }

        private bool AreEqual(Person actual, Person expected)
        {
            if (actual == expected) return true;
            if (actual == null || expected == null) return false;
            return
                actual.Name == expected.Name
                && actual.Age == expected.Age
                && actual.Height == expected.Height
                && actual.Weight == expected.Weight
                && AreEqual(actual.Parent, expected.Parent);
        }
    }

    public class TsarRegistry
    {
        public static Person GetCurrentTsar()
        {
            return new Person(
                "Ivan IV The Terrible", 54, 170, 70,
                new Person("Vasili III of Russia", 28, 170, 60, null));
        }
    }

    public class Person
    {
        public static int IdCounter = 0;
        public int Age, Height, Weight;
        public string Name;
        public Person Parent;
        public int Id;

        public Person(string name, int age, int height, int weight, Person parent)
        {
            Id = IdCounter++;
            Name = name;
            Age = age;
            Height = height;
            Weight = weight;
            Parent = parent;
        }
    }
}