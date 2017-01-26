
namespace Assets.Scripts.Behaviours
{
    public class PlayFieldUI : DefaultUI
    {
        public override void OnShow(bool immediately)
        {
            PlayField.Unlock();
            base.OnShow(immediately);
        }

        public override void OnHide(bool immediately)
        {
            PlayField.Lock();
            base.OnHide(immediately);
        }
    }
}
