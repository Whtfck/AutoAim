# AutoAim

这是一个为Dockov开发的自动瞄准模组，使用Harmony库进行方法补丁，实现玩家靠近敌人时，自动瞄准最近敌人的脑袋，但视野仍可自由转动。

## 功能

- 自动查找并锁定最近的敌人
- 支持两种方式获取敌人的头部位置
- 可以退化为身体中心瞄准

## 使用技术

- C#
- Harmony库进行方法补丁
- Unity引擎

## 主要类和方法

- `ModBehaviour`：继承自`Duckov.Modding.ModBehaviour`，负责初始化和卸载模组。
- `AutoAimPatch`：使用Harmony对`InputManager.SetAimInputUsingMouse`方法进行补丁，实现自动瞄准逻辑。
- `GetEnemyHeadPosition`：尝试通过`HeadCollider`组件或射线检测获取敌人的头部位置。

## 安装

1. 将`AutoAim.dll`和其他依赖项放入游戏的插件目录。
2. 确保已经安装了Harmony库。
3. 启动游戏，模组会自动加载。

## 贡献

欢迎贡献代码和提出改进建议。请确保遵循项目的编码规范和提交准则。

## 许可证

本项目遵循MIT许可证。详情请查看项目根目录的LICENSE文件。
