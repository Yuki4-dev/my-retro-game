using System;
using System.IO;

namespace SuperKiroWorld
{
    /// <summary>
    /// ハイスコアの保存と読み込みを管理するクラス
    /// </summary>
    public class ScoreManager
    {
        private const string SaveFileName = "highscore.dat";
        private string SaveFilePath { get; set; }
        
        public int HighScore { get; private set; }
        
        public ScoreManager()
        {
            // ゲーム実行ディレクトリのパスを取得
            SaveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFileName);
            HighScore = 0;
        }
        
        /// <summary>
        /// ハイスコアをファイルから読み込む
        /// </summary>
        public void LoadHighScore()
        {
            try
            {
                // ファイルが存在するかチェック
                if (!File.Exists(SaveFilePath))
                {
                    Console.WriteLine("ハイスコアファイルが存在しません。初期値0を使用します。");
                    HighScore = 0;
                    return;
                }
                
                // ファイルからスコアを読み込む
                string scoreText = File.ReadAllText(SaveFilePath).Trim();
                
                // 整数値に変換を試みる
                if (int.TryParse(scoreText, out int loadedScore))
                {
                    // 妥当性チェック（0以上の値のみ受け入れる）
                    if (loadedScore >= 0)
                    {
                        HighScore = loadedScore;
                        Console.WriteLine($"ハイスコアを読み込みました: {HighScore}");
                    }
                    else
                    {
                        Console.WriteLine($"無効なスコア値です（負の値）: {loadedScore}。初期値0を使用します。");
                        HighScore = 0;
                    }
                }
                else
                {
                    Console.WriteLine($"ハイスコアファイルが破損しています: {scoreText}。初期値0を使用します。");
                    HighScore = 0;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"ハイスコア読み込みエラー: {ex.Message}");
                HighScore = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"予期しないエラー: {ex.Message}");
                HighScore = 0;
            }
        }
        
        /// <summary>
        /// ハイスコアをファイルに保存
        /// </summary>
        /// <param name="score">保存するスコア</param>
        public void SaveHighScore(int score)
        {
            try
            {
                // 妥当性チェック
                if (score < 0)
                {
                    Console.WriteLine($"無効なスコア値です（負の値）: {score}。保存をスキップします。");
                    return;
                }
                
                // ファイルにスコアを書き込む
                File.WriteAllText(SaveFilePath, score.ToString());
                Console.WriteLine($"ハイスコアを保存しました: {score}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"ハイスコア保存エラー: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"予期しないエラー: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 現在のスコアがハイスコアを超えているかチェックして更新
        /// </summary>
        /// <param name="currentScore">現在のスコア</param>
        /// <returns>新記録の場合true、そうでない場合false</returns>
        public bool CheckAndUpdateHighScore(int currentScore)
        {
            if (currentScore > HighScore)
            {
                HighScore = currentScore;
                SaveHighScore(currentScore);
                Console.WriteLine($"新記録達成！ {currentScore}");
                return true;
            }
            
            return false;
        }
    }
}
