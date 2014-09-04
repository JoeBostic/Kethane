﻿using Kethane.GeodesicGrid;
using System;
using UnityEngine;

namespace Kethane
{
    public class BodyResourceData
    {
        private readonly IBodyResources resources;
        private readonly CellSet scans;

        protected BodyResourceData(IBodyResources resources, CellSet scans)
        {
            this.resources = resources;
            this.scans = scans;
        }

        public double MaxDepositQuantity
        {
            get { return resources.MaxQuantity; }
        }

        public ICellResource GetCellDeposit(Cell cell)
        {
            return resources.GetResource(cell);
        }

        public bool IsCellScanned(Cell cell)
        {
            return scans[cell];
        }

        public void ScanCell(Cell cell)
        {
            scans[cell] = true;
        }

        public static BodyResourceData Load(IResourceGenerator generator, CelestialBody body, ConfigNode bodyNode)
        {
            var resources = generator.Load(body, bodyNode.GetNode("GeneratorData"));
            var scans = new CellSet(MapOverlay.GridLevel);

            var scanMask = bodyNode.GetValue("ScanMask");
            if (scanMask != null)
            {
                try
                {
                    scans = new CellSet(MapOverlay.GridLevel, Misc.FromBase64String(scanMask));
                }
                catch (FormatException e)
                {
                    Debug.LogError(String.Format("[Kethane] Failed to parse {0} scan string, resetting ({1})", body.name, e.Message));
                }
            }

            return new BodyResourceData(resources, scans);
        }

        public void Save(ConfigNode bodyNode)
        {
            bodyNode.AddValue("ScanMask", Misc.ToBase64String(scans.ToByteArray()));

            var node = resources.Save() ?? new ConfigNode();
            node.name = "GeneratorData";
            bodyNode.AddNode(node);
        }
    }
}
