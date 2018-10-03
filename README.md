# sharkbot
a chatbot api that uses natural language processing and machine learning to talk like a person

## setup
- Open the SharkbotApi solution and change appsettings.json.  
- Update the NaturalLanguage settings to point to the files in \sharkbot\Data\
- Create a ConversationDirectory and UserDirectory
- run the SharkbotApi project
- train the bot by feeding it conversations using a client or by placing conversation json files in your database before you run it

## clients
- https://github.com/sharkson/angular-sharkbot
- https://github.com/sharkson/DiscordSpecialSharkbot

## performance
- The initial loading of the ConversationDirectory is much faster if it's located on an SSD compared to an HDD
- The more cores the faster it runs.  Sharkbot is heavily multi-threaded.
- It will churn through gigabytes of conversation data and reply in under a second even on a slow PC

## live example bot
- You can add a live version of sharkbot to your discord channel
- https://discordapp.com/oauth2/authorize?client_id=268518279809597453&scope=bot&permissions=0

## features
- analyzes conversations with natural language processing (NLP)
- learns in real time as it's fed conversation data
- responds to a message in a conversation using the context of the conversation, not just the message
- uses NLP to learn about users
- can naturally steer a conversation to a desired subject