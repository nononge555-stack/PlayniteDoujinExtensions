# Engine Detection

## Goal

登録されたゲームのディレクトリから、セーブ管理に必要な範囲でゲームエンジンを判定する。

エンジン判定そのものを目的にせず、正しいSave Locatorを安全に選択することを目的とする。

## Initial Scope

Phase 1:

- RPG Maker 2000
- RPG Maker 2003
- RPG Maker XP
- RPG Maker VX
- RPG Maker VX Ace
- RPG Maker MV
- RPG Maker MZ

それ以外は `Unknown` とし、セーブ管理を自動実行しない。

## Detection Principle

単一のファイル名だけで断定しない。

可能であれば複数の特徴を組み合わせる。

例:

- 実行ファイル名
- データディレクトリ構成
- データファイル拡張子
- `Game.ini`
- `www/` や `package.json`
- RPG Maker固有ファイル

判定結果には、将来的に以下を保持できる形が望ましい。

```text
Engine
Confidence
Evidence[]
```

## Save Locator Draft

標準構成の初期候補。

| Engine | 標準的なセーブ候補 |
| --- | --- |
| RPG Maker 2000/2003 | ゲームディレクトリの `Save*.lsd` |
| RPG Maker XP | ゲームディレクトリの `Save*.rxdata` |
| RPG Maker VX | ゲームディレクトリの `Save*.rvdata` |
| RPG Maker VX Ace | ゲームディレクトリの `Save*.rvdata2` |
| RPG Maker MV | ゲーム側 `save/` ディレクトリ |
| RPG Maker MZ | ゲーム側 `save/` ディレクトリ |

これは標準構成のヒューリスティックであり、作品側のRubyスクリプト、JavaScriptプラグイン、独自ランチャー等で変更される可能性がある。

そのためユーザー指定のSave Path/Patternを最優先できるようにする。

## Detection Priority

同一ゲームで複数候補が一致するケースを想定する。

暫定方針:

1. 明確な世代固有ファイルを優先
2. 複数証拠が一致するDetectorを優先
3. 信頼できない場合はUnknownへ倒す
4. 誤判定して自動バックアップ/削除するより、未判定を選ぶ

## User Override

ゲームごとに以下を上書き可能にする構想。

- Engine
- Save directory
- Include patterns
- Exclude patterns

自動判定の結果を永久に固定せず、再検出も可能にする。

## Future Engines

候補:

- WOLF RPG Editor
- NScripter
- KiriKiri / KAG
- TyranoScript
- Ren'Py
- Unity
- RPG Developer Bakin

ただし、非ツクール対応はEngineごとに保存仕様を調査し、十分安全に判定できるものから追加する。
