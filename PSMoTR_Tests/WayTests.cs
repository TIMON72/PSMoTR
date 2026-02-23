using System.Collections.Generic;
using PSMoTR.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSMoTR_Tests
{
    [TestClass]
    public class WayTests
    {
        [TestMethod]
        public void FindStartRoutesTest()
        {
            Route
                a = new Route(new Point((float)0.0, (float)0.0), "straight", new Point((float)0.0, (float)0.5)),
                b = new Route(new Point((float)0.0, (float)0.5), "straight", new Point((float)0.5, (float)0.0)),
                c = new Route(new Point((float)0.0, (float)0.0), "straight", new Point((float)0.5, (float)0.5));
            Route.RoutesList.Add(a);
            Route.RoutesList.Add(b);
            Route.RoutesList.Add(c);
            var actual = Way.FindStartRoutes();
            var excpected = new List<Route>() { a, c };
            CollectionAssert.AreEqual(excpected, actual);
            Route.RoutesList.Clear();
        }
    }
}