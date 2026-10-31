# Snap.Hutao.Server

胡桃的服务端。

## 静态资源分发（Snap.Hutao.Server.API）

客户端通过 API 的 `/static/**` 请求静态资源，服务端按 `x-hutao-quality` 选择变体后返回 302：

| 路由 | 请求头 | 跳转目标 |
|---|---|---|
| `GET /static/zip/{fileName}` | 未携带 / `Original` | `{StaticZipBaseUrl}/{fileName}` |
| `GET /static/zip/{fileName}` | `High` | `{StaticLowZipBaseUrl}/{fileName}`，默认 `{StaticZipBaseUrl}` 同级的 `low_zip` |
| `GET /static/raw/{category}/{fileName}` | 未携带 / `Original` | `{StaticRawBaseUrl}/{category}/{fileName}` |
| `GET /static/raw/{category}/{fileName}` | `High` | `{StaticLowRawBaseUrl}/{category}/{fileName}`，默认 `{StaticRawBaseUrl}` 同级的 `low` |
| `GET /static/size` | — | 读取 `{StaticSizeUrl}`，默认 `{StaticZipBaseUrl}` 父目录下的 `size.json` |

`App` 配置段（`appsettings.json`，可用 `App__...` 环境变量覆盖）：

- `StaticRawBaseUrl`、`StaticZipBaseUrl`（已有）
- `StaticLowRawBaseUrl`、`StaticLowZipBaseUrl`（可选，覆盖自动派生的 `low` / `low_zip`）
- `StaticSizeUrl`（可选，覆盖自动派生的 `size.json`）

行为说明：

- 只认 `x-hutao-quality: High`，其他值或缺省都返回原图；也接受 `?quality=High` 便于手工验证。
- 302 响应带 `Vary: x-hutao-quality`，避免中间缓存把一种变体复用给所有请求。
- `x-hutao-archive` 暂未实现（没有精简包体），保留给以后扩展。
- `/static/size` 拉不到清单时返回全 0 并记录 warning，客户端显示 `0 B`。
- 清单由 `StaticResourceBuilder` 生成，手动上传到 `<static-host>/static/size.json`。
