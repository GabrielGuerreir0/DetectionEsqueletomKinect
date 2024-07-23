# Programa de Rastreamento de Esqueleto com Kinect do Xbox

## Descrição do Projeto

Este projeto tem como objetivo desenvolver um programa para rastreamento de esqueleto utilizando o Kinect do Xbox. O sistema é capaz de detectar e rastrear os movimentos do corpo humano em tempo real, utilizando os dados capturados pelo sensor Kinect.

## Estrutura do Projeto

O projeto está organizado nas seguintes partes:

### Configuração do Ambiente

Antes de iniciar o desenvolvimento, é necessário configurar o ambiente com as seguintes ferramentas e bibliotecas:

- **Kinect SDK**: Software Development Kit fornecido pela Microsoft para interagir com o sensor Kinect.
- **Linguagem de Programação**: O projeto foi desenvolvido em C# para aproveitar as bibliotecas oferecidas pelo Kinect SDK.
- **IDE**: Visual Studio ou outra IDE compatível com desenvolvimento em C#.

### Classes Principais

O projeto contém as seguintes classes principais:

#### Classe `KinectSensorManager`

Gerencia a inicialização e o controle do sensor Kinect.

- **Métodos:**
  - `InitializeSensor()`: Inicializa o sensor Kinect.
  - `StopSensor()`: Para o sensor Kinect.
  - `GetSkeletonData()`: Obtém os dados do esqueleto capturados pelo sensor.

#### Classe `SkeletonTracker`

Responsável por processar os dados do esqueleto e realizar o rastreamento dos movimentos.

- **Métodos:**
  - `TrackSkeleton()`: Processa os dados do esqueleto e realiza o rastreamento.
  - `GetJointPosition()`: Retorna a posição de uma articulação específica.

#### Classe `MainWindow`

Interface gráfica do usuário (GUI) para visualizar os dados do rastreamento em tempo real.

- **Métodos:**
  - `UpdateSkeletonView()`: Atualiza a visualização do esqueleto na interface gráfica.

## Requisitos

- Kinect do Xbox com cabo de conexão para PC.
- Kinect SDK instalado.
- Visual Studio ou outra IDE compatível com C#.
- Windows 7 ou superior.

## Como Executar o Projeto

1. Clone este repositório para sua máquina local:
    ```bash
    git clone https://github.com/seu-usuario/projeto-kinect-rastreamento.git
    ```

2. Abra o projeto no Visual Studio.

3. Configure as dependências do Kinect SDK no projeto.

4. Conecte o Kinect do Xbox ao PC.

5. Compile e execute a aplicação.

## Exemplo de Uso

A interface gráfica permite visualizar o esqueleto rastreado em tempo real. Aqui está um exemplo de como o código pode ser utilizado:

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        KinectSensorManager sensorManager = new KinectSensorManager();
        sensorManager.InitializeSensor();

        SkeletonTracker tracker = new SkeletonTracker();
        tracker.TrackSkeleton(sensorManager.GetSkeletonData());

        Application.Run(new MainWindow());
    }
}
