using System.Collections.Generic;
using PSMoTR.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSMoTR_Tests
{
    [TestClass]
    public class RouteTests
    {
        [TestMethod]
        public void IsContainPointTest()
        {
            // Подготовка теста
            Route
                a = new Route(new Point((float)0.0, (float)0.0), "straight", new Point((float)0.0, (float)0.5)),
                b = new Route(new Point((float)0.0, (float)0.0), "straight", new Point((float)0.5, (float)0.0)),
                c = new Route(new Point((float)0.0, (float)0.0), "straight", new Point((float)0.5, (float)0.5));
            Route.RoutesList.Add(a);
            Route.RoutesList.Add(b);
            Route.RoutesList.Add(c);
            // Проверка результатов
            var actual = a.IsContainPoint(new Point(0, 0));
            var excpected = true;
            Assert.AreEqual(excpected, actual);
            actual = a.IsContainPoint(new Point(0, 1));
            excpected = false;
            Assert.AreEqual(excpected, actual);
            // Очистка данных тестирования
            Route.RoutesList.Clear();
        }

        [TestMethod]
        public void FindByPointsTest()
        {
            Route
                a = new Route(new Point((float)0.0, (float)0.0), "straight", new Point((float)0.0, (float)0.5)),
                b = new Route(new Point((float)0.0, (float)0.0), "straight", new Point((float)0.5, (float)0.0)),
                c = new Route(new Point((float)0.0, (float)0.0), "straight", new Point((float)0.5, (float)0.5));
            Route.RoutesList.Add(a);
            Route.RoutesList.Add(b);
            Route.RoutesList.Add(c);

            Assert.AreEqual(Route.FindByPoints(new Point((float)0.0, (float)0.0), new Point((float)0.0, (float)0.5)), a);
            Assert.AreEqual(Route.FindByPoints(new Point((float)0.0, (float)0.0), new Point((float)0.5, (float)0.0)), b);
            Assert.AreEqual(Route.FindByPoints(new Point((float)0.0, (float)0.0), new Point((float)0.5, (float)0.5)), c);
            Route.RoutesList.Clear();
        }

        [TestMethod]
        public void FindCrossingRoutesTest()
        {
            Route
                r1 = new Route(new Point(10, 10), "straight", new Point(20, 10)),
                r2 = new Route(new Point(30, 10), "straight", new Point(40, 10));
            Route.RoutesList.Add(r1);
            Route.RoutesList.Add(r2);
            // Тесты с горизонтальными отрезками
            // Тест 1 (на одной прямой без пересечения)
            var actual = Route.FindCrossingRoutes(r1);
            List<Route> excpected = new List<Route>() { };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 1");
            // Тест 2 (на разных прямых, но с пересечением по X)
            r1.Start = new Point(10, 10);
            r1.End = new Point(20, 10);
            r2.Start = new Point(15, 20);
            r2.End = new Point(20, 20);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 2");
            // Тест 3 (на одной прямой с пересечением)
            r1.Start = new Point(10, 10);
            r1.End = new Point(20, 10);
            r2.Start = new Point(15, 10);
            r2.End = new Point(30, 10);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { r2 };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 3");
            // Тест 4 (на одной прямой с пересечением в общем конце)
            r1.Start = new Point(10, 10);
            r1.End = new Point(20, 10);
            r2.Start = new Point(20, 10);
            r2.End = new Point(30, 10);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { r2 };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 4");
            // Тест 5 (наложение друг на друга)
            r1.Start = new Point(10, 10);
            r1.End = new Point(20, 10);
            r2.Start = new Point(10, 10);
            r2.End = new Point(20, 10);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { r2 };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 5");
            // Вертикальные прямые
            // Тест 6 (на одной прямой без пересечения)
            r1.Start = new Point(10, 10);
            r1.End = new Point(10, 20);
            r2.Start = new Point(10, 30);
            r2.End = new Point(10, 40);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 6");
            // Тест 7 (на разных прямых с пересечением по Y)
            r1.Start = new Point(10, 10);
            r1.End = new Point(10, 20);
            r2.Start = new Point(20, 15);
            r2.End = new Point(20, 30);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 7");
            // Тест 8 (на одной прямой с пересечением)
            r1.Start = new Point(10, 10);
            r1.End = new Point(10, 20);
            r2.Start = new Point(10, 15);
            r2.End = new Point(10, 30);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { r2 };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 8");
            // Тест 9 (на одной прямой с пересечением в общем конце)
            r1.Start = new Point(10, 10);
            r1.End = new Point(10, 20);
            r2.Start = new Point(10, 20);
            r2.End = new Point(10, 30);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { r2 };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 9");
            // Тест 10 (наложение)
            r1.Start = new Point(10, 10);
            r1.End = new Point(10, 20);
            r2.Start = new Point(10, 10);
            r2.End = new Point(10, 20);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { r2 };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 10");
            // Одна прямая - горизонтальна, а другая - наклонная
            // Тест 11 (расположены на расстоянии)
            r1.Start = new Point(10, 10);
            r1.End = new Point(20, 10);
            r2.Start = new Point(30, 5);
            r2.End = new Point(40, 15);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 11");
            // Тест 12 (расположены не пересекаячь c потенц. областью пересечения)
            r1.Start = new Point(10, 10);
            r1.End = new Point(20, 10);
            r2.Start = new Point(11, 15);
            r2.End = new Point(5, 5);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 12");
            // Тест 13 (имеют общий конец)
            r1.Start = new Point(10, 10);
            r1.End = new Point(20, 10);
            r2.Start = new Point(15, 15);
            r2.End = new Point(10, 10);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { r2 };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 13");
            // Тест 14 (пересекаются)
            r1.Start = new Point(10, 10);
            r1.End = new Point(20, 10);
            r2.Start = new Point(20, 10);
            r2.End = new Point(20, 20);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { r2 };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 14");
            // Обе прямые - наклонные
            // Тест 15 (не имеют ничего общего)
            r1.Start = new Point(10, 10);
            r1.End = new Point(20, 20);
            r2.Start = new Point(30, 30);
            r2.End = new Point(60, 10);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 15");
            // Тест 16 (имеют потенц. область пересечения, но не пересекаются)
            r1.Start = new Point(10, 10);
            r1.End = new Point(50, 15);
            r2.Start = new Point(30, 30);
            r2.End = new Point(20, 20);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 16");
            // Тест 17 (имеют общий конец)
            r1.Start = new Point(50, 50);
            r1.End = new Point(30, 30);
            r2.Start = new Point(30, 30);
            r2.End = new Point(20, 20);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { r2 };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 17");
            // Тест 18 (пересекаются)
            r1.Start = new Point(30, 30);
            r1.End = new Point(50, 50);
            r2.Start = new Point(40, 40);
            r2.End = new Point(20, 20);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { r2 };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 18");
            // Одна прямая - горизонтальная, а другая - вертикальная
            // Тест 19 (пересекаются в точке, являющейся концом)
            r1.Start = new Point(40, 40);
            r1.End = new Point(50, 40);
            r2.Start = new Point(40, 30);
            r2.End = new Point(40, 50);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { r2 };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 19");
            // Тест 20 (пересекаются в общей точке концов)
            r1.Start = new Point(40, 40);
            r1.End = new Point(50, 40);
            r2.Start = new Point(50, 40);
            r2.End = new Point(60, 60);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { r2 };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 20");
            // Тест 21 (не пересекаются, но имеют потенц. область пересечения)
            r1.Start = new Point(40, 40);
            r1.End = new Point(50, 40);
            r2.Start = new Point(39, 30);
            r2.End = new Point(39, 50);
            actual = Route.FindCrossingRoutes(r1);
            excpected = new List<Route>() { };
            CollectionAssert.AreEqual(excpected, actual, "Ошибка в тесте 21");
            Route.RoutesList.Clear();
        }
    }
}