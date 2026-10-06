using System.Collections.Generic;
namespace Broad
{
    public class CommandBuffer
    {
        Queue<ICommand> commands = new Queue<ICommand>();


        /// <summary>
        /// 現在のコマンド数
        /// </summary>
        public int CommandCount => commands.Count;

        /// <summary>
        /// コマンドを追加
        /// </summary>
        /// <param name="cmd">コマンド</param>
        public void Add(ICommand cmd)
        {
            commands.Enqueue(cmd);
        }


        /// <summary>
        /// 全てのコマンドを実行
        /// </summary>
        public void Execute()
        {
            while (commands.Count > 0) {
                commands.Dequeue().Execute();
            }
        }
    }
}
