using System;

namespace GarageGroup.Infra;

[AttributeUsage(AttributeTargets.Property)]
public sealed class DbFieldIgnoreAttribute : Attribute
{
}