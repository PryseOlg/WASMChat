using System;

namespace WASMChat.CodeGenerators.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class InjectScoped : Attribute
{ }