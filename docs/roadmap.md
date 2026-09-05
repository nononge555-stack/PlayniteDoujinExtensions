# Roadmap

## Phase 0 - Repository / Design

- [x] プロジェクトコンセプト整理
- [x] Core / Playnite / Storeの責務分離
- [x] セーブ安全要件の明文化
- [x] RPG Maker初期対応範囲の決定
- [ ] ライセンス方針確定
- [ ] Playnite Toolboxから最新Generic Pluginテンプレート生成
- [ ] Solution / Project作成
- [ ] CI作成

## Phase 1 - DoujinTools MVP

目標: Playniteに既に登録されているツクールゲームのセーブを手動管理できる。

- [ ] `Doujin.Core` 作成
- [ ] Engine model作成
- [ ] RPG Maker 2000 detector
- [ ] RPG Maker 2003 detector
- [ ] RPG Maker XP detector
- [ ] RPG Maker VX detector
- [ ] RPG Maker VX Ace detector
- [ ] RPG Maker MV detector
- [ ] RPG Maker MZ detector
- [ ] Save Locator
- [ ] Backup service
- [ ] Restore service
- [ ] Manifest
- [ ] Playnite Generic Plugin作成
- [ ] `Backup Save` メニュー
- [ ] `Restore Save` メニュー
- [ ] ゲーム単位のEngine/Save Path上書き
- [ ] Unit tests

## Phase 2 - Automatic Backup / Archive

目標: プレイ終了後のセーブ保護と、安全なゲーム本体削除を実現する。

- [ ] Playnite game stoppedイベント連携
- [ ] 変更検出
- [ ] 自動バックアップ
- [ ] SHA-256等による検証
- [ ] バックアップ世代管理
- [ ] Backup History UI
- [ ] Archive manifest
- [ ] `Archive & Remove`
- [ ] 削除前確認UI
- [ ] バックアップ失敗時に削除されないことのテスト
- [ ] 復元前の既存セーブ保護

## Phase 3 - DLsite

目標: DLsite作品をPlayniteライブラリへ自然に統合する。

- [ ] 既存Playnite DLsite拡張調査
- [ ] 既存OSSとライセンス調査
- [ ] 現行DLsite仕様調査
- [ ] Metadata Provider
- [ ] Library Plugin
- [ ] RJ番号管理
- [ ] 購入作品同期
- [ ] ローカルインストールとの紐付け
- [ ] カバー/サークル/タグ等の取得

## Phase 4 - FANZA / DMM

- [ ] 既存Playnite FANZA拡張調査
- [ ] 現行サイト仕様・利用条件調査
- [ ] Metadata Provider
- [ ] Library Plugin
- [ ] 購入作品同期
- [ ] ローカルインストールとの紐付け

## Phase 5 - Install Lifecycle

目標: アーカイブ済みゲームを再び遊ぶまでの手順を短縮する。

- [ ] Storeからの再取得支援
- [ ] 展開/インストール管理
- [ ] Engine再検出
- [ ] 保存済みセーブの存在通知
- [ ] セーブ自動復元
- [ ] RTP / ランタイム支援

## Future

候補であり、初期実装を複雑にしない。

- WOLF RPG Editor
- NScripter
- KiriKiri / KAG
- TyranoScript
- Ren'Py
- Unity系ゲームの限定的なSave Locator
- RPG Developer Bakin
- バックアップ保存先のクラウド同期支援
- 複数PC間でのセーブ管理
- ゲームバージョン/アップデートとセーブスナップショットの関連付け
