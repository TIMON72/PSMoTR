using System.Collections.Generic;
using PSMoTR.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSMoTR_Tests
{
    [TestClass]
    public class TrafficLightTests
    {
        private static TrafficLight TL1 { get; set; } = new TrafficLight("X=100,5; Y=68; Speed=8; Type=up; GD=25; RD=25; SSC=Green; OL=70; LS=False");
        private static TrafficLight TL2 { get; set; } = new TrafficLight("X=100,5; Y=71,5; Speed=8; Type=usual; GD=25; RD=25; SSC=Green; OL=70; LS=False");
        private static TrafficLight TL3 { get; set; } = new TrafficLight("X=107,5; Y=58; Speed=0; Type=usual; GD=25; RD=25; SSC=Red; OL=70; LS=False");
        private static TrafficLight TL4 { get; set; } = new TrafficLight("X=110,5; Y=58; Speed=0; Type=right; GD=25; RD=25; SSC=Red; OL=70; LS=False");
        private static TrafficLight TL5 { get; set; } = new TrafficLight("X=110,5; Y=78; Speed=0; Type=left; GD=25; RD=25; SSC=Red; OL=70; LS=False");
        private static TrafficLight TL6 { get; set; } = new TrafficLight("X=113,5; Y=78; Speed=0; Type=usual; GD=25; RD=25; SSC=Red; OL=70; LS=False");
        private static TrafficLight TL7 { get; set; } = new TrafficLight("X=120,5; Y=65; Speed=8; Type=usual; GD=25; RD=25; SSC=Green; OL=70; LS=True");
        private static TrafficLight TL8 { get; set; } = new TrafficLight("X=120,5; Y=68; Speed=8; Type=down; GD=25; RD=25; SSC=Green; OL=70; LS=False");

        private static Route R1 { get; set; } = new Route("64 68 12 straight 106 68 8");
        private static Route R2 { get; set; } = new Route("64 71,5 12 straight 103 71,5 8");
        private static Route R3 { get; set; } = new Route("103 65 8 straight 64 65 16");
        private static Route R4 { get; set; } = new Route("103 71,5 8 curveX 107,5 76 8");
        private static Route R5 { get; set; } = new Route("103 71,5 8 straight 118 71,5 8");
        private static Route R6 { get; set; } = new Route("106 68 8 curveX 113,5 60,5 8");
        private static Route R7 { get; set; } = new Route("107,5 22 12 straight 107,5 60,5 8");
        private static Route R8 { get; set; } = new Route("107,5 60,5 8 curveY 103 65 8");
        private static Route R9 { get; set; } = new Route("107,5 60,5 8 straight 107,5 76 8");
        private static Route R10 { get; set; } = new Route("107,5 76 8 straight 107,5 114,5 16");
        private static Route R11 { get; set; } = new Route("110,5 22 12 straight 110,5 63,5 8");
        private static Route R12 { get; set; } = new Route("110,5 63,5 8 curveY 118 71,5 8");
        private static Route R13 { get; set; } = new Route("110,5 72,5 8 curveY 103 65 8");
        private static Route R14 { get; set; } = new Route("110,5 114,5 12 straight 110,5 72,5 8");
        private static Route R15 { get; set; } = new Route("113,5 60,5 8 straight 113,5 22 16");
        private static Route R16 { get; set; } = new Route("113,5 76 8 curveY 118 71,5 8");
        private static Route R17 { get; set; } = new Route("113,5 76 8 straight 113,5 60,5 8");
        private static Route R18 { get; set; } = new Route("113,5 114,5 12 straight 113,5 76 8");
        private static Route R19 { get; set; } = new Route("115 68 8 curveX 107,5 76 8");
        private static Route R20 { get; set; } = new Route("118 65 8 curveX 113,5 60,5 8");
        private static Route R21 { get; set; } = new Route("118 65 8 straight 103 65 8");
        private static Route R22 { get; set; } = new Route("118 71,5 8 straight 130,5 71,5 12");
        private static Route R23 { get; set; } = new Route("130,5 65 12 straight 118 65 8");
        private static Route R24 { get; set; } = new Route("130,5 68 12 straight 115 68 8");

        [TestMethod]
        public void FindDependentTLsTest()
        {
            // Инициализация входных данных
            TrafficLight.TLsList = new List<TrafficLight> { TL1, TL2, TL3, TL4, TL5, TL6, TL7, TL8 };
            Route.SetRoutesList(new List<Route> { R1, R2, R3, R4, R5, R6, R7, R8, R9, R10,
                                                 R11, R12, R13, R14, R15, R16, R17, R18, R19, R20,
                                                 R21, R22, R23, R24 });
            // Инициализация свойств
            TrafficLight.Initialization();
            // Тест 1
            var actual = TL1.ParallelTLsList;
            var expected = new List<TrafficLight>() { TL2, TL7, TL8 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 1.1");
            actual = TL1.PerpendicularTLsList;
            expected = new List<TrafficLight>() { TL3, TL4, TL5, TL6 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 1.2");
            // Тест 2
            actual = TL2.ParallelTLsList;
            expected = new List<TrafficLight>() { TL1, TL7, TL8 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 2.1");
            actual = TL2.PerpendicularTLsList;
            expected = new List<TrafficLight>() { TL3, TL4, TL5, TL6 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 2.2");
            // Тест 3
            actual = TL3.ParallelTLsList;
            expected = new List<TrafficLight>() { TL4, TL5, TL6 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 3.1");
            actual = TL3.PerpendicularTLsList;
            expected = new List<TrafficLight>() { TL1, TL2, TL7, TL8 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 3.2");
            // Тест 4
            actual = TL4.ParallelTLsList;
            expected = new List<TrafficLight>() { TL3, TL5, TL6 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 4.1");
            actual = TL4.PerpendicularTLsList;
            expected = new List<TrafficLight>() { TL1, TL2, TL7, TL8 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 4.2");
            // Тест 5
            actual = TL5.ParallelTLsList;
            expected = new List<TrafficLight>() { TL3, TL4, TL6 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 5.1");
            actual = TL5.PerpendicularTLsList;
            expected = new List<TrafficLight>() { TL1, TL2, TL7, TL8 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 5.2");
            // Тест 6
            actual = TL6.ParallelTLsList;
            expected = new List<TrafficLight>() { TL3, TL4, TL5 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 6.1");
            actual = TL6.PerpendicularTLsList;
            expected = new List<TrafficLight>() { TL1, TL2, TL7, TL8 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 6.2");
            // Тест 7
            actual = TL7.ParallelTLsList;
            expected = new List<TrafficLight>() { TL1, TL2, TL8 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 7.1");
            actual = TL7.PerpendicularTLsList;
            expected = new List<TrafficLight>() { TL3, TL4, TL5, TL6 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 7.2");
            // Тест 8
            actual = TL8.ParallelTLsList;
            expected = new List<TrafficLight>() { TL1, TL2, TL7 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 8.1");
            actual = TL8.PerpendicularTLsList;
            expected = new List<TrafficLight>() { TL3, TL4, TL5, TL6 };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 8.2");
            // Очистка данных
            TrafficLight.TLsList.Clear();
            Route.RoutesList.Clear();
        }

        [TestMethod]
        public void FindInfluentialRoutesTest()
        {
            // Инициализация входных данных
            TrafficLight.TLsList = new List<TrafficLight> { TL1, TL2, TL3, TL4, TL5, TL6, TL7, TL8 };
            Route.SetRoutesList(new List<Route> { R1, R2, R3, R4, R5, R6, R7, R8, R9, R10,
                                                 R11, R12, R13, R14, R15, R16, R17, R18, R19, R20,
                                                 R21, R22, R23, R24 });
            // Инициализация свойств
            TrafficLight.Initialization();
            // Тест 1
            var actual = TL1.FindInfluentialRoutes();
            var excpected = new List<Route>() { R9, R12, R13, R17, R20, R21 };
            CollectionAssert.AreEquivalent(excpected, actual, "Ошибка в тесте 1");
            // Тест 2
            actual = TL2.FindInfluentialRoutes();
            excpected = new List<Route>() { R9, R12, R13, R16, R17, R19 };
            CollectionAssert.AreEquivalent(excpected, actual, "Ошибка в тесте 2");
            // Тест 3
            actual = TL3.FindInfluentialRoutes();
            excpected = new List<Route>() { R21, R13, R6, R5, R4, R19 };
            CollectionAssert.AreEquivalent(excpected, actual, "Ошибка в тесте 3");
            // Тест 4
            actual = TL4.FindInfluentialRoutes();
            excpected = new List<Route>() { R21, R6, R19, R17, R5, R16 };
            CollectionAssert.AreEquivalent(excpected, actual, "Ошибка в тесте 4");
            // Тест 5
            actual = TL5.FindInfluentialRoutes();
            excpected = new List<Route>() { R5, R19, R6, R9, R21, R8 };
            CollectionAssert.AreEquivalent(excpected, actual, "Ошибка в тесте 5");
            // Тест 6
            actual = TL6.FindInfluentialRoutes();
            excpected = new List<Route>() { R5, R12, R19, R21, R6, R20 };
            CollectionAssert.AreEquivalent(excpected, actual, "Ошибка в тесте 6");
            // Тест 7
            actual = TL7.FindInfluentialRoutes();
            excpected = new List<Route>() { R17, R6, R12, R9, R13, R8 };
            CollectionAssert.AreEquivalent(excpected, actual, "Ошибка в тесте 7");
            // Тест 8
            actual = TL8.FindInfluentialRoutes();
            excpected = new List<Route>() { R17, R12, R13, R5, R9, R4 };
            CollectionAssert.AreEquivalent(excpected, actual, "Ошибка в тесте 8");
            // Очистка данных
            TrafficLight.TLsList.Clear();
            Route.RoutesList.Clear();
        }

        [TestMethod]
        public void SignalTimingTest()
        {
            // Инициализация входных данных
            TrafficLight.TLsList = new List<TrafficLight> { TL1, TL2, TL3, TL4, TL5, TL6, TL7, TL8 };
            Route.SetRoutesList(new List<Route> { R1, R2, R3, R4, R5, R6, R7, R8, R9, R10,
                                                 R11, R12, R13, R14, R15, R16, R17, R18, R19, R20,
                                                 R21, R22, R23, R24 });
            // Инициализация свойств
            TrafficLight.Initialization();
            // Тест 1
            TL1.GD = 10;
            var actual = new List<int>() { 10, 25, 25, 25, 25, 25, 25, 25 };
            var expected = new List<int>() { TL1.GD, TL2.GD, TL3.RD, TL4.RD,
                                             TL5.RD, TL6.RD, TL7.GD, TL8.GD };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 1");
            // Тест 2
            TL1.GD = 10;
            TL2.GD = 15;
            actual = new List<int>() { 10, 15, 25, 25, 25, 25, 25, 25 };
            expected = new List<int>() { TL1.GD, TL2.GD, TL3.RD, TL4.RD,
                                             TL5.RD, TL6.RD, TL7.GD, TL8.GD };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 2");
            // Тест 3
            TL1.GD = 10;
            TL2.GD = 15;
            TL7.GD = 10;
            actual = new List<int>() { 10, 15, 25, 25, 25, 25, 10, 25 };
            expected = new List<int>() { TL1.GD, TL2.GD, TL3.RD, TL4.RD,
                                             TL5.RD, TL6.RD, TL7.GD, TL8.GD };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 3");
            // Тест 4.1
            TL1.GD = 10;
            TL2.GD = 15;
            TL7.GD = 10;
            TL8.GD = 5;
            actual = new List<int>() { 10, 15, 15, 15, 15, 15, 10, 5 };
            expected = new List<int>() { TL1.GD, TL2.GD, TL3.RD, TL4.RD,
                                             TL5.RD, TL6.RD, TL7.GD, TL8.GD };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 4.1");
            // Тест 4.2
            TL3.RD = 10;
            TL4.RD = 15;
            TL5.RD = 5;
            TL6.RD = 10;
            actual = new List<int>() { 5, 5, 10, 15, 5, 10, 5, 5 };
            expected = new List<int>() { TL1.GD, TL2.GD, TL3.RD, TL4.RD,
                                             TL5.RD, TL6.RD, TL7.GD, TL8.GD };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 4.2");
            // Тест 5
            TL1.GD = 5;
            TL2.GD = 5;
            TL7.GD = 15;
            TL8.GD = 20;
            actual = new List<int>() { 5, 5, 20, 20, 20, 20, 15, 20 };
            expected = new List<int>() { TL1.GD, TL2.GD, TL3.RD, TL4.RD,
                                             TL5.RD, TL6.RD, TL7.GD, TL8.GD };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 5.1");
            TL8.GD = 10;
            actual = new List<int>() { 5, 5, 15, 15, 15, 15, 15, 10 };
            expected = new List<int>() { TL1.GD, TL2.GD, TL3.RD, TL4.RD,
                                             TL5.RD, TL6.RD, TL7.GD, TL8.GD };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 5.2");
            // Тест 6
            TL3.RD = 50;
            TL4.RD = 60;
            TL5.RD = 70;
            TL6.RD = 70;
            actual = new List<int>() { 50, 50, 50, 60, 70, 70, 50, 50 };
            expected = new List<int>() { TL1.GD, TL2.GD, TL3.RD, TL4.RD,
                                             TL5.RD, TL6.RD, TL7.GD, TL8.GD };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 6.1");
            TL3.RD = 80;
            actual = new List<int>() { 60, 60, 80, 60, 70, 70, 60, 60 };
            expected = new List<int>() { TL1.GD, TL2.GD, TL3.RD, TL4.RD,
                                             TL5.RD, TL6.RD, TL7.GD, TL8.GD };
            CollectionAssert.AreEquivalent(expected, actual, "Ошибка в тесте 6.2");
            // Очистка данных
            TrafficLight.TLsList.Clear();
            Route.RoutesList.Clear();
        }
    }
}
