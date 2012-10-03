namespace System.Linq.Dynamic
{
    interface ISubtractSignatures : IAddSignatures
    {
        void F(DateTime x, DateTime y);
        void F(DateTime? x, DateTime? y);
    }
}
