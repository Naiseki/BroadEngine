# BroadEngine

C#製のシンプルな2Dゲームエンジンです。

ゲームエンジンの内部構造や設計を理解することを目的として開発しました。

> [!NOTE]
> このプロジェクトは現在メンテナンスしておらず、今後の機能追加・不具合修正・サポートは予定していません。

## 特徴

- Unityライクなシーン/アクタ/コンポーネント構成
- OpenTK（OpenGL）による描画
- スプライト/タイルマップ描画
- 物理演算
- アセット管理
## リポジトリ構成 

- `BroadEngine/` : エンジン本体（ライブラリ）
- `MathKit/` : 数学ライブラリ
- `EngineTest/` : サンプル（実行プロジェクト）

## 必要要件

- .NET SDK 6.0

## ビルド

### エンジン（ライブラリ）

- `BroadEngine/BroadEngine.csproj`

### サンプル（EngineTest）

サンプルは `EngineTest/EngineTest.csproj` です。

## 実行（EngineTest）

- `dotnet run --project EngineTest/EngineTest.csproj`

## 使い方（最小例）

1. `Scene` を継承してゲームループを記述
2. `Actor` を生成し、`Component`（例: `SpriteRenderer`, `RigidBody`）を追加
3. `Game.Run(settings, firstSceneType)` で起動

エントリポイント例は `EngineTest/Program.cs` を参照してください。

## アセット（EmbeddedResource）

`EngineTest` では `Resources/**` を埋め込みリソースとして同梱しています（`EngineTest.csproj` の `EmbeddedResource` 参照）。

エンジン側の `Assets` API で読み込みます。

- `Assets.RegisterAssets(...)` で登録
- `Assets.Load<T>(address)` でロード
- `Assets.UnloadUnusedAssets()` で未使用アセットを破棄

## クレジット

本プロジェクトでは表示フォントに「源真ゴシック」(http://jikasei.me/font/genshin/) を使用しています。

Licensed under SIL Open Font License 1.1 (http://scripts.sil.org/OFL)

© 2015 自家製フォント工房, © 2014, 2015 Adobe Systems Incorporated, © 2015 M+

FONTS PROJECT
