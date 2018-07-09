# BrainTumorPredictViewer

## Overview

- TensorFlow를 사용한 뇌종양 예측 프로그램의 시각화
 
 ## Role
 	 
- 정재우: 기존 만들어 둔 뇌종양 예측 코드 및 테스트 이미지 준비
- 김종필: python 파일의 실행 및 테스트 이미지 및 실행 결과 폴더에 대한 Viewer 준비

## To do

- python 실행 결과 log 출력
- mha -> png 변환 작업을 위한 folder select UI

## Development Environment

### Python & Anaconda & TensorFlow

#### Anaconda 5.2 with Python 3.5.5

- Download link[https://www.anaconda.com/download/](https://www.anaconda.com/download/)

#### Install TensorFlow 1.8.0

``` python
> pip install tensorflow
```

``` python
> python
> import tensorflow as tf
> tf.__version__
'1.8.0'
```

#### Activate tensorflow

``` python
(base)> activate tensorflow
(tensorflow)>
```

#### pip check and upgrade 10.0.1

``` python
> python -m pip install --upgrade pip
```

#### Install matplotlib

``` python
> pip install matplotlib
```

#### Install tensor

``` python
> pip install tensor
```

#### Install tensorlayer

``` python
> pip install tensorlayer
```

#### Install OpenCV

``` python
> pip install OpenCV-Python
```

### Visual Studio Community 2017

[https://www.visualstudio.com/downloads/](https://www.visualstudio.com/downloads/)

### .NET Desktop Environment

- WPF
- .NET Framework 4.7.1