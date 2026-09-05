# Save Management

## Purpose

ゲーム本体は削除・再取得できても、ユーザーのセーブデータは再取得できない。

そのため本プロジェクトでは、セーブデータをゲーム本体より重要なユーザーデータとして扱う。

## MVP Policy

MVPではセーブファイル内部の解析や編集を行わない。

必要なのは以下のみ。

- セーブ対象ファイル/ディレクトリを特定する
- バックアップする
- バックアップ結果を検証する
- 必要に応じて復元する

## Backup Storage Draft

```text
SaveArchive/
`-- <Playnite Game ID>/
    |-- metadata.json
    |-- current/
    |   |-- Save01.rvdata2
    |   `-- Save02.rvdata2
    `-- history/
        |-- 2026-09-05_170000/
        `-- 2026-09-01_220000/
```

実際の保存先は設定可能にする。

ネットワークドライブ、クラウド同期フォルダ等を将来的に選択可能にしてもよいが、初期実装では通常のローカルフォルダを優先する。

## Manifest Draft

```json
{
  "schemaVersion": 1,
  "gameId": "<Playnite Game ID>",
  "engine": "RpgMakerVxAce",
  "createdAt": "2026-09-05T17:00:00+09:00",
  "files": [
    {
      "relativePath": "Save01.rvdata2",
      "size": 123456,
      "sha256": "..."
    }
  ]
}
```

manifestは将来変更できるよう `schemaVersion` を持たせる。

## Backup Flow

```text
Locate save data
      |
Build backup plan
      |
Copy to temporary/staging area
      |
Verify copied files
      |
Write manifest
      |
Promote backup as valid
```

途中で失敗したバックアップを「正常なバックアップ」として扱わない。

## Archive & Remove Safety Contract

`Archive & Remove` は以下を満たした場合のみゲーム本体削除へ進める。

- Save Locatorが正常終了している
- 保存対象がある場合、すべてコピー済み
- コピー先ファイルが存在する
- ファイルサイズが一致する
- 設定された検証方式で整合性確認済み
- manifest保存済み
- バックアップがValid状態として確定済み

いずれかが失敗した場合は削除を中止する。

セーブ対象が0件の場合は「セーブが存在しない」のか「検出できなかった」のかを区別する。Unknown Engineや判定不能時に、自動的に「セーブなし」と決めつけて削除しない。

## Restore Policy

復元先に既存セーブが存在する場合は上書き前に保護する。

初期案:

1. 既存セーブを検出
2. 復元前バックアップを作成
3. ユーザーに復元対象を表示
4. 復元
5. 復元結果を検証

無確認の破壊的上書きは避ける。

## Automatic Backup

Playniteのゲーム終了イベントを利用して、対応Engineでセーブ変更があれば自動バックアップする構想。

差分判定は最初から高度にせず、mtime/size/hash等の組み合わせを実装時に検討する。

自動バックアップ失敗によってゲーム終了処理やPlaynite自体を不安定にしないこと。

## Retention

セーブデータは通常ゲーム本体より非常に小さいため、複数世代を保持する。

世代数、保存期間、最大容量は将来設定可能にする。

削除対象となる古いバックアップでも、最低1つのValidバックアップを残すことを基本方針とする。
