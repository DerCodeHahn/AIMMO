start C:\Users\Hahn\Anaconda3\Scripts\activate.bat ml-agents
echo mlagents-learn config/trainer_config.yaml --run-id=firstRun --train
echo tensorboard --logdir=summaries

Start "" cmd /k "activate ml-agents"


pause