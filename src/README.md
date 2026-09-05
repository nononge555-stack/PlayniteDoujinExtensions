# Source Layout

実装開始後は以下のモジュールを配置する。

```text
src/
|-- Doujin.Core/
|   |-- Engines/
|   |-- Saves/
|   |-- Archives/
|   |-- Models/
|   `-- Storage/
|-- DoujinTools/
|-- DLsite/
`-- FANZA/
```

## Doujin.Core

Playnite非依存のエンジン判定・セーブ管理・アーカイブロジック。

## DoujinTools

Playnite Generic Plugin。Phase 1/2の中心。

## DLsite

DLsite Library / Metadata integration。Phase 3で追加。

## FANZA

FANZA/DMM Library / Metadata integration。Phase 4で追加。

実装開始時はPlaynite Toolboxの最新テンプレートを基準にプロジェクトを生成し、不要なテンプレートコードを整理してからコミットする。
