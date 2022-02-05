namespace Subject
{
    public class Foo
    {
        private readonly IBar _bar;
        public int Number { get; set; }

        public Foo(IBar bar)
        {
            _bar = bar;
        }

        public virtual int FooMethod(int i)
        {
            return Number = _bar.BarMethod(i);
        }
    }
}