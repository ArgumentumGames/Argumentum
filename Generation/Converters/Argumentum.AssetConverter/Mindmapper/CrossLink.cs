using System;

namespace Argumentum.AssetConverter.Mindmapper;

[Flags]
public enum CrossLink
{
	None = 0,
	Identity = 1 << 0, // 1
	Opposite = 1 << 1, // 2
	AppealTo = 1 << 2, // 4
	Symmetric = 1 << 3, // 8
}