# Neuron Simulation Project

Welcome to the Neuron Simulation Project! This project simulates the activity of neurons and their connections, visualizing the neuroplasticity and interactions between neurons in real-time using Unity.

![Neuron Simulation](images/neuron_simulation.png)

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Introduction

This project aims to simulate the behavior of neurons using a real-time 3D environment in Unity. It provides an interactive experience where users can see how neurons communicate and change over time, demonstrating principles of neuroplasticity.

## Features

- **Real-time Visualization**: Watch neurons communicate and change their activity levels.
- **Customizable Settings**: Adjust the simulation parameters such as neuron sensitivity and connection strength.
- **Event-Driven Design**: Modular approach using events for clean and maintainable code.
- **Particle System Visualization**: Visualize neuron activity and spike paths using particles.

**Neuron Settings**

<img src="images/neuron_settings1.png" alt="Neuron Settings" width="400"/>

**Effect Settings**

<img src="images/neuron_settings2.png" alt="Neuron Settings" width="400"/>

To add instructions for setting up Newtonsoft.Json and JsonSubTypes to your README, you can update it with the following section:

---

## Installation

### Cloning the Repository

1. Clone the Repository:
    ```bash
    git clone https://github.com/yourusername/neurons.git
    ```

2. Open in Unity:
    - Make sure you have Unity installed.
    - Open the project in Unity by selecting the `neuron-simulation` folder.

### Setting Up Newtonsoft.Json and JsonSubTypes

1. **Add Newtonsoft.Json via Unity Package Manager:**
   - Open Unity and navigate to **Window** > **Package Manager**.
   - Click the **+** button in the top left and choose **Add package from git URL**.
   - Enter: `https://github.com/jilleJr/Newtonsoft.Json-for-Unity.git#upm`.
   - Click **Add**.

2. **Add JsonSubTypes:**
   - Download the JsonSubTypes DLL from the [GitHub repository](https://github.com/manuc66/JsonSubTypes/releases).
   - Place the DLL in the `Assets/Plugins` folder of your Unity project.

---


## Usage

1. **Run the Simulation:**
   - Open the Unity Editor and hit the `Play` button to start the simulation.
   - Observe the neuron activity and explore the visualization tools.

2. **Customize Settings:**
   - Access the `NeuronManager` to adjust simulation parameters.
   - Use the UI to start, pause, or reset the simulation.

3. **Saving and Loading:**
   - Use the save/load buttons in the UI to store and retrieve simulation states.

![Saving and Loading Interface](path/to/your/third_image.png)

## Contributing

We welcome contributions! Please follow these steps:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes and commit them (`git commit -m 'Add new feature'`).
4. Push to the branch (`git push origin feature-branch`).
5. Open a pull request.

Please read our [contribution guidelines](CONTRIBUTING.md) for more details.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
