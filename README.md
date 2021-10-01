# [GodSeekerPlus](https://github.com/Clazex/HollowKnight.GodSeekerPlus)

A Hollow Knight mod to enhance your Godhome experience

Compatible with `Hollow Knight` 1.5.
**`Vasi` is required.**

## Features

- **Fast Dream Warping**: Remove dream warping charge time when in Godhome boss fight rooms. This decrease the total warping time from 2.25s to 0.25s.
- **Frame Rate Limit**: Create a lag in every in-game frame.

## Configuration

- `fastDreamWarp` (`Boolean`): Whether to enable the Fast Dream Warping feature. Defaults to `true`.
- `frameRateLimit` (`Boolean`): Whether to enable the Frame Rate Limit feature. Defaults to `false`.
- `frameRateLimitMultiplier` (`Integer`): Frame rate limit time span multiplier. Final lag time is 10ms multiplied by this value. Note that setting this to `0` does not mean zero lag. Ranges from `0` to `10`, defaults to `5`.

## Contributing

1. Clone the repository
2. Set environment variable `HollowKnightRefs` to your `Managed` folder in HK installation

