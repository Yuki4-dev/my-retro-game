# 🔨 Build Instructions

このドキュメントでは、Super Kiro Worldをスタンドアロン実行可能ファイル（EXE）としてビルドする方法を説明します。

## 📋 前提条件

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) がインストールされていること

## 🚀 クイックスタート

### Windows ユーザー向け

#### 方法1: バッチスクリプト（最も簡単）

1. プロジェクトのルートディレクトリで `build-exe.bat` をダブルクリック
2. ビルドが完了するまで待つ
3. `build/win-x64/SuperKiroWorld.exe` または `build/win-x86/SuperKiroWorld.exe` を実行

#### 方法2: PowerShellスクリプト

```powershell
# PowerShellでプロジェクトのルートディレクトリに移動
cd path\to\SuperKiroWorld

# スクリプトを実行
.\build-exe.ps1
```

### Linux / macOS ユーザー向け

```bash
# プロジェクトのルートディレクトリに移動
cd path/to/SuperKiroWorld

# Linux用にビルド
dotnet publish SuperKiroWorld/SuperKiroWorld.csproj \
  -c Release \
  -r linux-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -o ./build/linux-x64

# macOS用にビルド
dotnet publish SuperKiroWorld/SuperKiroWorld.csproj \
  -c Release \
  -r osx-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -o ./build/osx-x64
```

## 🎯 ビルドオプション詳細

### サポートされているランタイム

- `win-x64` - Windows 64ビット
- `win-x86` - Windows 32ビット
- `linux-x64` - Linux 64ビット
- `osx-x64` - macOS 64ビット (Intel)
- `osx-arm64` - macOS ARM64 (Apple Silicon)

### ビルドパラメータの説明

- `-c Release` - リリースモードでビルド（最適化有効）
- `-r [runtime]` - ターゲットランタイムを指定
- `--self-contained true` - .NETランタイムを含める（インストール不要）
- `-p:PublishSingleFile=true` - 単一の実行可能ファイルとして出力
- `-p:IncludeNativeLibrariesForSelfExtract=true` - ネイティブライブラリを含める
- `-p:EnableCompressionInSingleFile=true` - ファイルサイズを圧縮
- `-o [path]` - 出力ディレクトリを指定

## 📦 配布用パッケージの作成

### Windows

```cmd
# ビルド後、ZIPファイルを作成
cd build\win-x64
tar -a -c -f SuperKiroWorld-Windows-x64.zip *
```

### Linux / macOS

```bash
# ビルド後、ZIPファイルを作成
cd build/linux-x64
zip -r ../SuperKiroWorld-Linux-x64.zip .
```

## 🤖 CI/CD (GitHub Actions)

このプロジェクトには自動ビルド用のGitHub Actionsワークフローが含まれています。

### 自動ビルド

- `main` または `master` ブランチへのプッシュで自動的にビルド
- ビルド成果物はGitHub Actionsのアーティファクトとしてダウンロード可能

### リリースの作成

```bash
# タグを作成してプッシュ
git tag v1.0.0
git push origin v1.0.0
```

または、GitHub Actionsの「Create Release」ワークフローを手動で実行。

## 🔍 トラブルシューティング

### ビルドが失敗する

1. .NET 8.0 SDKがインストールされているか確認
   ```bash
   dotnet --version
   ```

2. 依存関係を復元
   ```bash
   dotnet restore SuperKiroWorld/SuperKiroWorld.csproj
   ```

3. クリーンビルドを試す
   ```bash
   dotnet clean SuperKiroWorld/SuperKiroWorld.csproj
   dotnet build SuperKiroWorld/SuperKiroWorld.csproj
   ```

### 実行ファイルが大きすぎる

- `--self-contained false` を使用してランタイムを含めない（ただし、実行には.NET 8.0が必要）
- または、`-p:PublishTrimmed=true` を追加して未使用コードを削除（注意: MonoGameでは問題が発生する可能性あり）

### Linux/macOSで実行権限がない

```bash
chmod +x SuperKiroWorld
./SuperKiroWorld
```

## 📊 ビルドサイズの比較

| ビルドタイプ | サイズ（概算） |
|------------|--------------|
| Self-contained (圧縮あり) | ~50-70 MB |
| Self-contained (圧縮なし) | ~80-100 MB |
| Framework-dependent | ~5-10 MB |

## 🎮 ビルド後のテスト

ビルドが成功したら、以下を確認してください：

1. ✅ ゲームが起動する
2. ✅ グラフィックが正しく表示される
3. ✅ 入力（キーボード）が機能する
4. ✅ サウンドが再生される（実装されている場合）
5. ✅ ハイスコアが保存される
6. ✅ エフェクトが正しく表示される

## 📝 追加情報

- ビルドされた実行ファイルは、.NETランタイムがインストールされていないPCでも動作します
- 初回起動時、Windowsが「不明な発行元」の警告を表示する場合があります（正常な動作です）
- ゲームデータ（ハイスコアなど）は実行ファイルと同じディレクトリに保存されます

---

**Happy Building!** 🎮✨
