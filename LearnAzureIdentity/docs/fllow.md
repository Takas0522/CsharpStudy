``` mermaid
sequenceDiagram
    participant C as クライアント
    participant A1 as Azure ADアプリケーション
    participant W as Webアプリケーション
    participant K as Key Vault
    C ->> A1: アクセストークンを生成
    C ->> W: 生成したアクセストークンを利用してアクセス
    W ->> A1: 送信されたアクセストークンとシークレットを利用してKVへのアクセストークンを取得
    A1 -->> W: アクセストークン返却
    W ->> K: アクセス可能に
```