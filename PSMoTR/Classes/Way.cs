using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMoTR.Classes
{
    public class Way
    {
        public Route StartRoute { get; set; }
        public List<Route> RoutesList { get; set; } = new List<Route>();
        public static List<Way> WaysList { get; set; } = new List<Way>();
        public static int StartRoutesCount { get; set; } = 0; // Количество стартовых маршрутов
        /// <summary>
        /// Конструкторы
        /// </summary>
        public Way()
        {
            RoutesList = null;
            StartRoute = null;
        }
        public Way(Route startRoute)
        {
            RoutesList = new List<Route>
            {
                startRoute
            };
            StartRoute = startRoute;
        }
        public Way(List<Route> routesList)
        {
            RoutesList = routesList;
        }
        public Way(Route startRoute, List<Route> routesList)
        {
            RoutesList = routesList;
            StartRoute = startRoute;
        }
        /// <summary>
        /// Поиск стартовых отрезков (точек входа для авто)
        /// </summary>
        /// <returns></returns>
        public static List<Route> FindStartRoutes()
        {
            List<Route> startRoutesList = new List<Route>();
            for (int i = 0; i < Route.RoutesList.Count; i++)
            {
                bool ok = true;
                for (int j = 0; j < Route.RoutesList.Count; j++)
                {
                    if (i != j && (Route.RoutesList[i].Start == Route.RoutesList[j].End || Route.RoutesList[i].Start.Equals(Route.RoutesList[j].End)))
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok)
                    startRoutesList.Add(Route.RoutesList[i]);
            }
            StartRoutesCount = startRoutesList.Count;
            return startRoutesList;
        }
        /// <summary>
        /// Заполнение списка всех возможных путей
        /// </summary>
        public static void FillWaysList()
        {
            // Ищем все варианты маршрутов от стартовых и добавляем такие варианты в коллекцию путей
            List<Route> startRoutesList = FindStartRoutes();
            for (int i = 0; i < startRoutesList.Count; i++)
                FindAllWays(new Way(startRoutesList[i]), startRoutesList[i]);
        }
        /// <summary>
        /// Поиск всех возможных путей от текущего маршрута (рекурсия)
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="way"></param>
        /// <param name="startRoute"></param>
        private static void FindAllWays(Way way, Route currentRoute)
        {
            List<Route> routes_next = Route.RoutesList.FindAll(r => currentRoute.End == r.Start);
            // Следующий маршрут найден?
            if (routes_next.Count > 0)
            {
                for (int i = 0; i < routes_next.Count; i++)
                {
                    // Следующий маршрут уже есть в пути? (исключение замкнутых путей)
                    Route similarRoute = way.RoutesList.Find(r => r == routes_next[i]);
                    if (similarRoute != null)
                        return;
                    // Записываем следующий маршрут в путь
                    way.RoutesList.Add(routes_next[i]);
                    currentRoute = routes_next[i];
                    // Следующий маршрут - один? : выполняем поиск следующего
                    if (routes_next.Count == 1)
                    {
                        FindAllWays(way, currentRoute);
                        return;
                    }
                    else
                    {
                        // Запоминаем предыдущий путь (точка разделения)
                        Way prevWay = new Way
                        {
                            RoutesList = new List<Route>(way.RoutesList),
                            StartRoute = routes_next[i]
                        };
                        prevWay.RoutesList.RemoveAt(prevWay.RoutesList.Count - 1);
                        // Выполняем поиск следующего маршрута от точки разделения
                        FindAllWays(way, currentRoute);
                        // Делаем текущим путем предыдущий (до точки разделения)
                        way.RoutesList = prevWay.RoutesList;
                    }
                }
            }
            // Иначе записываем путь в коллекцию
            else
            {
                Way newWay = new Way(way.StartRoute, way.RoutesList);
                WaysList.Add(newWay);
                return;
            }
        }
        /// <summary>
        /// Проверка на пересечение маршрута
        /// </summary>
        /// <param name="targetRoute"></param>
        /// <returns></returns>
        public bool IsCrossingRoute(Route targetRoute)
        {
            List<Route> crossingRoutes = Route.FindCrossingRoutes(targetRoute);
            if (crossingRoutes.Count > 0)
            {
                var result = RoutesList.Intersect(crossingRoutes);
                if (result.Count() > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}