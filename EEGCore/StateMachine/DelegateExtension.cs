namespace EEGCore.StateMachine
{
    public static class DelegateExtension
    {
        public static T? FindTarget<T>(this IEnumerable<Delegate> delegates, StateBase? targetState) where T : Delegate
        {
            var targetDelegate = delegates?.Where(d => d.Target == targetState)
                                           .FirstOrDefault() as T;
            return targetDelegate;
        }
    }
}
