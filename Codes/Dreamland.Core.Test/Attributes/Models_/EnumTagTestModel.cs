namespace Dreamland.Core.Test.Attributes
{
    internal enum EnumTagTestModel
    {
        [EnumTag(Name = "one", Description = "numb = 1")]
        One,

        [EnumTag(Name = "two", Description = "numb = 2")]
        Two
    }
}