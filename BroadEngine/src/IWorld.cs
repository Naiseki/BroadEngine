namespace Broad
{
    public interface IWorld: IDestroyable
    {
        /// <summary>
        /// ワールドを更新
        /// </summary>
        void Step();
    }
}