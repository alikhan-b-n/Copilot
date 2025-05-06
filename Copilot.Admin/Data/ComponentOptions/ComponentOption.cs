namespace Copilot.Admin.Data.ComponentOptions
{
    public class ComponentOption<TContext>
    {
        public Guid Id { get; set; } = Guid.Empty;
        public TContext Context { get; set; }
        public bool IsPost { get; set; }
    }
}