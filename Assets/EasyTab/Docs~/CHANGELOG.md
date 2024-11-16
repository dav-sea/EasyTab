# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.3.1] - 2024-11-16
### 👨‍🔧 Fixed
- Exception when keyboard is not connected (#7, #8)

## [1.3.0] - 2024-10-26
### 🚨 Breaking changes in API
- Removed conditions from **EasyTabDriver**. Now you can use API based on decoration of `IEasyTabDriver`.
- Removed `TransformDriver`. Now you can use API based on decoration of `IEasyTabDriver`.

### 💡 Added
- Fast navigation by <kbd>Tab</kbd> key holding.

### 🚀 Improved
- Removed all allocations during navigation operation

### 🛠 Changed
- Refactoring and simplification `EasyTabDriver`

## [1.2.0] - 2024-08-11
### 🚀 Improved
- Dropdown support. Items inside `Dropdown` are automatically navigated cyclically (Roll)

### 👨‍🔧 Fixed
- Incorrect clamping and navigation in some cases (#3)

## [1.1.0] - 2024-07-20
### 💡 Added
- Feature: Conditions (#2)

### 🛠 Changed
- Refactoring and simplification `EasyTabNodeDriver` 
- Simplification `SceneDriver`
- Rework `TransformDriver`: remove UnityEngine.UI deps., add conditions support
- Add number-values for enums

### ✂️ Removed
- Removed `EasyTabDriver` for `EasyTab` component

## [1.0.2] - 2024-07-03
### 🚀 Improved
- Optimized: getting root GameObject`s without allocations (#1)
- Small improves in package.json and README.md

## [1.0.1] - 2024-06-25
### 💡 Added
- License 
- Info about OpenUPM installation method in README.md

### 🛠 Changed
- Move README.md to 'Assets/EasyTab/Docs~' folder
- Change `name` property in package.json   

## [1.0.0] - 2024-05-26
### 💡 Added
- Initial release