# sharkbot
a chatbot api that uses natural language processing and machine learning

## setup
- Open the SharkbotApi solution and change appsettings.json.  
- Update the NaturalLanguage settings to point to the files in \sharkbot\Data\
- Create a ConversationDirectory and UserDirectory
- Add conversations to the ConversationDirectory (example coming later)
- run the SharkbotApi project

## performance
- The initial loading of the ConversationDirectory is much faster if it's located on an SSD compared to an HDD
- The more cores the faster it runs.  Sharkbot is heavily multi-threaded.