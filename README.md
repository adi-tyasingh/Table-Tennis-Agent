# Table Tennis Agent:

### Description: 
The aim of this project is to utilize, Unity along with the Unity ML agents module and Python package to train an Agent to play table tennis. 
The Unity ML agents module provides a wide range of features that make it easy to train working ML agents using Reinforcement Learning. 

### Technologies Used: 
- Unity 2020.3.19f1
- [Unity Ml agents Module](https://github.com/Unity-Technologies/ml-agents)
- Anaconda 
- PyTorch
- Open Neural Network

### Helpful Resources: 
- https://github.com/Unity-Technologies/ml-agents
- https://www.youtube.com/watch?v=D0jTowlMROc
- https://docs.unity3d.com/Packages/com.unity.ml-agents@3.0/manual/index.html

### Creating a RL agent using ML agents: 
- Firstly we create the TTAgent class by extending the Agent class defined in ML agents
- We override the following methods:
  - Initialize: to initialize the agent
  - OnEpisodeBegin: setting the position and other values of the agent at the beginning of the episode
  - CollectObservation: collect observations from the environment and create a vector of it, to be passed to the Neural Network
  - Heuristic (For testing controls) 
  - OnActionRecieved: To update the position and velocity of the agent(bat) on receiving action as output from the neural net.
 - We also need to create a YAML file to define the hyperparameters for the agent's training.
 - The other files and classes handle Game mechanics and rewards. 

### Acknowledgements: 
I am very thankful to the creators for creating such a powerful tool and providing it to us for free. I am also thankful to the creators of the Unity ML agents Module without which the implementation of this project would have been impossible. 





