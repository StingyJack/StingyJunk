namespace StingyJunk.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public partial class ConsoleWindow
    {
        private const string DEFAULT_DISPLAY_AREA = "Default";

        public DisplayArea DefaultArea => new DisplayArea(DEFAULT_DISPLAY_AREA,
            0, 0, Console.WindowHeight, Console.WindowWidth)
        {
            Cycle = false,
            Scroll = true
        };

        private void AddDisplayAreasIn(IEnumerable<DisplayArea> areasToUse)
        {
            foreach (var a in areasToUse)
            {
                _displayAreas.Add(a.Name, a);
            }
        }

        private void EnsureDisplayAreasAreDistinct()
        {
            foreach (var daKey in _displayAreas.Keys)
            {
                var currentDisplayArea = _displayAreas[daKey];
                var notThisArea = _displayAreas.Where(d => d.Key.Equals(daKey) == false).ToList();
                var overlappingTops = notThisArea.Where(o => o.Value.Top <= currentDisplayArea.Top && o.Value.Bottom >= currentDisplayArea.Top).ToList();
                if (overlappingTops.Any())
                {
                    throw new ArgumentException($"Display area {daKey} overlaps top in area(s) {overlappingTops.Select(k => k.Key).ToCsl()}");
                }
                var overlappingBottoms = notThisArea.Where(o => o.Value.Top <= currentDisplayArea.Bottom && o.Value.Bottom >= currentDisplayArea.Bottom).ToList();
                if (overlappingTops.Any())
                {
                    throw new ArgumentException($"Display area {daKey} overlaps bottom in area(s) {overlappingBottoms.Select(k => k.Key).ToCsl()}");
                }
            }
        }

        // ReSharper disable UnusedParameter.Local
        private void VerifyDisplayAreas(string displayAreaName)
            // ReSharper restore UnusedParameter.Local
        {
            if (DisplayAreas.Count > 1
                && (string.IsNullOrWhiteSpace(displayAreaName) || DEFAULT_DISPLAY_AREA.Equals(displayAreaName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("DisplayArea must be named when more than one are registered");
            }
        }
    }
}