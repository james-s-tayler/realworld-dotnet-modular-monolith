using System.Reflection;
using App.Core.PipelineBehaviors.Authorization;
using App.Core.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace App.Core.Modules
{
    // from: https://medium.com/@austin.davies0101/creating-a-basic-authorization-pipeline-with-mediatr-and-asp-net-core-c257fe3cc76b
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthorizersFromAssembly(
            this IServiceCollection services,
            Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var authorizerType = typeof(IAuthorizer<>);
            assembly.GetTypesAssignableTo(authorizerType).ForEach((type) =>
            {
                foreach ( var implementedInterface in type.ImplementedInterfaces )
                {
                    switch ( lifetime )
                    {
                        case ServiceLifetime.Scoped:
                            services.AddScoped(implementedInterface, type);
                            break;
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(implementedInterface, type);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(implementedInterface, type);
                            break;
                    }
                }
            });
        }
        /*
         *                                                                                                                .''''.      .''''. .''''...'''.  .''''.    .;:ccc:'   .''''. .'''''. .''''.                                                                                                                
                                                                                                                'ONNNNx.    'ONNNNx'oNNNNd:ONNNK;.oNNNNK; .dXWMMMMMNO;.xNNNNo.lNNNNNd.cNNNNO'                                                                                                               
                                                                                                                '0MMMMx.    '0MMMMklKMMMK,,0MMMMk'oWMMMX;.kMMMM0kNMMMK;lWMMMk'dMMMMMx.dMMMMk.                                                                                                               
                                                                                                                '0MMMMx.    '0MMMW00WMMNl ,0MMMMNddWMMMX;'0MMMWl;KMMMWocXMMMO:kMMMMMk;kMMMWo                                                                                                                
                                                                                                                '0MMMMx.    '0MMMMWWMMWk. ,0MMMMMX0WMMMX;,KMMMWl;XMMMMo,0MMM0o0MMMMM0dKMMMNc                                                                                                                
                                                                                                                '0MMMMx.    '0MMMMMMMMNc  ,0MMMMMWWWMMMX;;XMMMWl;XMMMMo'kMMMKOXMMMMMX0XMMMK,                                                                                                                
                                                                                                                '0MMMMx.    '0MMMMMMMMWx. ,0MMMWWMMMMMMX;;XMMMWl;KMMMMo.oMMMNXNMWXNMNXNMMMO.                                                                                                                
                                                                                                                '0MMMMx.    '0MMMMNNMMMX: ,0MMMXXMMMMMMX;,KMMMWl;XMMMMd cWMMWWWMXxKMWWWMMMx.                                                                                                                
                                                                                                                '0MMMMx.    '0MMMMK0WMMMO.,KMMMOdNMMMMMX;'0MMMWl;KMMMWo ;XMMMMMM0ckMMMMMMWl                                                                                                                 
                                                                                                                '0MMMMx.    '0MMMMkoXMMMWc,0MMMx,kMMMMMX;.OMMMMOxNMMMX: .OMMMMMMx.oMMMMMMX:                                                                                                                 
                                                                                                                '0WWWWx.    '0WWWWx;kWWWWOl0WWWk.:XWWWMX; 'kNMMMMMMW0:  .xWWMWWWo :XWWWWW0'                                                                                                                 
                                                                                                                 ',,,,.      ',,,,. .,,,,,.',,,.  ',,,,'.   ':llllc,.    .,,,,,,. .',,,,,'                                                                                                                  
                 ...                                         .............................................................................................  . ...............................................,:ccccllcccllllccccccccccccclcccccccccccccccccc:::::::::::::;;;;;;;;;;;,,,,'...                
          .............        ...................................'''''',,,;;;;;;:::::::ccccccccccccccccccccccccccccclcllccc:::::::::cccc::;;;,,,,,,'....................''...''''''''''........'''''',,,;cokO000000000000KK000000000000KKK00000KKKKKKKK0000OOOOOOOkkkkkkkkkxxxxxxddddoooc:,....            
    ...................         .................................'''',,,,,;;;;:::::ccccclllllllllllllllccccccccccllllllllloolllccccccccllllc::;;,,,''''...............................'............''',,,,:oxO000000000OO0000000000OOOOOOO000000000000KKK000OOOOkkkkkkxxxxxxxxxxdddddooooolllc;,....           
    ..................          .............................''',,,,,;;;;;:::::cccccccllllllllllllllllllllcccccccllllllooooooolllllllllllllcc:;;,,'''.............................................'',,;;:ldk00000000000OO0000000000OOOOOOOOOOO000000000KK000OOkkkkkkxxxxxddxxxdddddddoololllccc:;'....          
    ...............            .........................'''',,,,;;;;;::::::cccccccccclllllllllooooolllllllcccccclllllloooooooooollllllllollc::;,''............      .............................';::coxO0K000OOOO00000000000000OOOOOOOOOOOOOO000000000000OOOkkkkkkxxxxxxdddddddooooollllllccc::;,....          
    .............             .................''''''''',,,,,;;;:::::ccccccccccccccccllllllllloooollllllllccccccllloooooddoooooooooooooooolc:;,''...........               ...................',,;:cldk0KK00000000000000000000OOOOOOOOOOOOOOOOO0000000000OOOkkkkkxxxxxxxddddddddoooollllccccc:::;,'...          
    ............             ........'''''''''''',,,,,,,;;;;;::::cccccccccccccccccccccccllllllllllllllllllccccllllooooddddooooooooooooooooolc::;;,,''.............     .....................',;:::ccloxO00000000000000000000OOOOOOOOOOOOOOOOOOO000000OOOOkkkkxxxxxxxddddddooooooollccccc:::::;;;;,'....         
    ..........             ......'''''''''''',,,,,,,;;;;;:::::::ccccccccccccccccccccccccccclllllllllllllllccclllloooooddddoooooooooodddooooollcc:::;;;,,''''''.............'''''''''',,,,,,,;coxdlc::codkO0000000000000000000OOOOOOOOOOOOOOOO000000OOkkkkxxxdddddddddooooooolllllcc:;;;;;;;;;;;;,''....         
    .........             .....''''''''',,,,,,,;;;;;;;;;:::::::ccccccccccc::::::::::cccccccccccclllllllllcccllllloooooodooooooooooooooooooooolllcccc:::;;;;;;,,'''''''''',,,,,;;;;;;;;:::::cdkO0Okdc::::cokO00000000000000000OOOOOOO0OOOOOO000000OOkkxxxddddddddddoddoolllllllcc::;,'',,,,,,,,,,''.....         
    .......             ......''''''',,,,,,,,,;;;;;;;;::::::::::ccccccc:::::::::::::::::::::::cccccccccccccclllllllooooooooooooooooooooooooooollllllccc:::::;;;;;,,,,;;;;;;::::ccccccccccclx000000Odc;,,,:oxOO00OO00000000000000OO00000000000000OOkxxdddoodooooooooolllcccc:cc::;;,'..'''''''.........          
    .....             ........''''',,,,,,,,,,;;;;;;;;;;::::::::::::::::::::::;;;;;;;::;;;;;;;:::::::::ccccccclllllllllllloollllllllllllllolllllllcccc:::::::::;;;;;;;;::::::cccllllllllllox00000000Odl:,',:oxkOOOOOO000000000000000000000000000Okkxxddooooooooooolllccccc:::::;,,,'......'''.........           
                .........'''''',,,,,,,,;;;;;;;;;;;;;;;;;:::::::::::::;;;;;;;;;;;;;;;;;;;;;;;;;;;:::::::::ccccccccccllllllllllllllcccccccc::::::::::::::;;;;;::;;::::::ccclllllllllllldkOOOOOOO00Oxoc:;;codxkOOOOOO00000000000000000000000Okkxxdddddooooooooolllccccccccc:;,,'...'....''....                 
              ........''''',,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;::;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;::::::::ccccccccccccccccc:::::::;;;;;;;;;;;;;;;;;;;;;:::::::::ccccllllllccclxkkkkkxxxkOOOxdollllodxkOOOOOO0000000000000000000OOkkxddddddddddoooolllllcccccc::;;,,,,'''..........                   
            ........''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;;,,,,,,,,,;;;,,,,;;,;;;;;;;;;;;;;;;;;;;;;;;;::::::::::::::::::::;;;;;;;;;;;;;;;;;;;;;;;;;;;::::::::::cccccc::::coooodxOOkxddxxxdoooooxkOOOOOO000000000000000000OOkxdoooddxxxxxxddoooolllllllcc::::;;,;,'........                       
          ......'''''',,,,'',,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;,,,;,,,,,;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;,;;;;;;;;;;;;;;;;;;;;;;;,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;::;;;:::::::;;::cc:::::cdxkdoooooooodxkOOOOOOO0000OOOOOO000000Okkxdoloodxkkkkxxxddoooooolllllcc::::;,,'......                           
        .....'',,,,,,,,,,,''',,,,,,,,,,,,''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;:odolll;',;;:lllllooodxkOO0000OOOOkkkkkkkOOO000Okkxdoodxkkkkkxxxddoooolllllccccc::::;;,,'....                             
      ....'',,,,,,,,,,,,,''''''''''''''''''''',,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,cdkkxxo;';l:..';:cclodxkOOO00OOOOkxxxxxxxxxxkOOOkkkkxxxkkkkkxxdddooooollllccc:::::;;;;,,''.....                            
     ....'',,,,,,,,,,,,,,''''''''''''''''''''''''''',,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;,,,,,,,,,,,,,,,,,,,,,,,,'',,,,,,,,,,,,,,,,,''''''''''''''''''',,,,,,,:ok00Okkl,','.   .,:clodkOOO000OOkxddoooddddooooooddddxxkkkkkxxdooooollllllcccc::::;;;;,,''.......                           
     ....'',,,,,,,,,,,,,''''''''''''''''''''',,',''',,,,,,,,,,,,,,,,,,,;;;;;;;;;,,,,,,,,,;;;;;;;;;;;;;;;;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'''''''''''''''''',,,,''',,',;cdO000OOOkdc;'......':lodkOO000OOkxdoollllooollc:;;;:::cllodddddddoooollloollccc::::;;,,,'''.........                          
       .....''',,,,,,,''''''''''''''''''''''',,,,,,,,,,,,,,,,,,,,,,,,,,,,;,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'',,''''''''''''''''''''''''''',,',:ok0KK0000OOOOkxoc:,',;coxkO00OOOOkkdolc::;::cccc::;,'....',,;::cccloooooooooolllcc::;;,,''........                               
          .....'''','',,''''''''''''''''''''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,''''''''''''''''''''''',,,,,,:lxO0000000OOOOOOOOOOkxddkO0000OOOkxddoc:;,''.'',;;;;;;,'.........',,,;:cclllllllcccc:;;,,''......                                   
             ....''''',,'''''''''''''''''''''''''''''',,,,,,'',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,''''''''''''''''''''',,',:lxO0000000OOOOO00OOOOOOOOO00OOOOkkxdolc:;,.........',;;;;,'..............',,,;;;:::;;;;;,,,'......                                     
              ....''''''''''''''''''''''''''''''''''''''''''''''''''''''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,''''''''''''''''''''',;ok0K0000OOO00OO0000OOOOOOOOOOOkkxdollc:,'....      ...',;;,,,,''.....       ...'''''''',,,'''.....                                       
              .....''''''''''''''''''''''''''''''''''''''''''''''''''''''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,',,''''''''''''''''''''''''''';ok00000OOOOOOO0O000000000OOOOkxdolc:;;,,'....         ....'''',,,,,''......    ....................                                        
              ......'''''''''',,,,'''''''''''''''''''''''''''''''',,,,,'',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,''',,''''''''''''''''''''''''',ck0K00OOOOOOOOO000000000OOOkkxoc:;,''.......               ...'',,;;;;;;;;;,,'...................                                            
            ..........''''''',,,''''''''''''''''''''''''''''''''''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'',,,'''''''''''''''''''''''',ckKK0000OOOOOOOOO00000OOkxxdoc:,'......                     ..,,;;::ccccccccc:;,'....         ...                                             
    ..................'''''',,,,,,,''',,,,,,,,,,,,,,''''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'''''',,'',,,,''''''''''''''''''''''',lOK000OOOOOOOOOOO000OOkxolc:;,'...                           .;cclllllllllllccc::;,'...                                                        
    ................'''''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'''''''''''''''''''''''''''''''''''';oO0000OO0000000OOOOkxxolc:,'....                            .':looddxxxddolccccc::;,,'...                                                       
    ..........'''''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,''''''''''''''''''''''''''''''''''',oO00O0OO000000OOkxdollc:;'....                               ..,;::lodxkkkdolc:::;;;,'''...                                                      
    .....''''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'''''''''''''''''''''''''''''''''',lO00OOOOOO00OOkxdolc:;,''....                                    ..,;;;:ldxdolc:;;;;,,'''....                                                     
    '''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'''''''''''''''''''''''.............'lO00Okkkkkkkxdllc::;,'....                                  ......'cddc'.,,,;::;;,',,,'''.....                                                     
    '',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'''''''''''''''''''''...............'ck0OOkkxxddolc:;,,'.....                                    .';coolldko,'::.  ..'''....''......                                                     
    ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,',,,'''''''''''''.................,ck0OOOOxdolc:;,,'.....                                ......';:loxOOkxd:....     .....  .........                                                    
    ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'''',,,,,,,,,'''''''''''...................cxk00OOOOkdlc;,,'.......                               .....'';coddxxkkkxl,..              ........                                                    
    ',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'''''''''''''''''''.....................lO000OOOOOOxl:,,''........                             ...'',,:codddxxxdddoc'.....            ......                                                    
    '',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'''',,,,,''''''''''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'''''''''''''''..................... ..lOK000OOOOOOkdc;,'............                         ...',;;:codddxxxddollc:;'...             ....                                                     
    '','''',,,,,,,,',,,,'''',,,,,,,,,,,,,,,,,,,,,,,,,,,'''''''''''''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,''''''''''''..............  ......  ..l0K00000OOOOOOxoc;'..     ..''..                       ...',,;:clodxxxxxxdolc::;,,'...            ...                                                     
    '''''''''',,,,,',,,'''''',,,,''',,,,,,,,,,''',,,,,,''''''''''''''',,,,,,,,,,,,'',,,,'''',,,,,,,,,,,,,,,''''''''''''''..............    ....... .'o0K0000000000OOOxo:'..       .''..                      ..',,;:ccldxxxxxxddolc:;,,'.....            .                                                      
    ''''''''''''''''''''''''',,,'''',,,,,,,,,,''''''''''''''''''''''''''''',,''''''',,''''''',,,,,,,,,,,,''''''''''''...............         ......'d00000000000000OOkdc,..        .''.                    ...',,;::clodxkkkxxxdolcc:;,'.....                                                                   
    ''''''''''''''''''''''''',,''',,,,,,,,,,,,,,'''''''''''''''''''''''''''''''''''''''''''''',,,,''''''''''''''''............ ...             ...,d0000000000000000OOxo:,..        .'..                   ..'',;;::cllodxxxxxddoolcc:;,''....      ..                                                          
    '''''''''''''''''''''''''',,,,,,,,,,,,,,,,,,'''''''''''''''''''''''''''''''''''',,,,,,,,,,,,,''''''''''''''''..........                 ... .:x000000000000OOOO0Okxoc;'...      .''.                  ...',,;;::cclloddxdddddoolcc::;;,'...........                                                         
    ''''''''''''''''''''''''''''''',,,,,,,,'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''............                  ...'lxkOkkOOO000OOOkOO00Okdlc;,'...     .''..                 ...'',,;::cclloooddddddoollccc:;;;,'''......                                                          
    ''''''''''''''''''''''''''''''',,,,,,,,'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''..........                     ....:ddoddxxkkOOOOOOOO00Okdolc:;,'...   ..''..                 ..'',,;;:::cclloooooooolllccc:;;;,,''......                                                           
    .'''''''''''''''''''''''''''''',,,,,',,,'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''.........                   .     ...;ccccloodxkOOOO00000Okdolcc:;,'....  ......                ...'',,;;;:::cclllllllllcc:::;;,,,'''....                                                              
    ..''''''''''''''''''''''''''''',,,,,,,,,,,,,,,,''''''''''''''''''''''''.'''''''''...'''''''''''''............                       ..  .cl:;;;:ccloxxkkOO000Okdolcc:;;,''....  ...                 ....'',,,;;;;::cccccccccc:::;,,,'''.....                                                                
    ....'''''''''''''''''''''''''''',,,,,,,,,,,,,,,,''''''''''''''....''''''''''''''...''''''''''''............                       .. ..'lko:;;,,;::ccloodxkkkkxolcc:;;,,''...........               ...'',,,,,,,;;;::::::::;;;;;;,,,''......                                                                
    ......'''''..'''''''''''''''''',,,,,,,,,,,,,,,,,'''''''''''''..''..'''''''''''''..''''''''''''..........                           ...'okxl;,''..'',;:::cclllooolllcc:;,''...............    ..........'''''',,,,,;;;;;;;;;,,,,,,,,,'''.......                                                              
    ......'''''..'''''''''''''''''''',,,,,,,,,,,,,,''''''''''''''.''....''''''''''''''''''''''''..........                            ...,dOOxl;'......';;,,;;;;;::cccccc:;'......................'........'''''''''',,,,,,,,,,'''''''''''.........                                                             
    .....''''''..''''''''''''''''''''',,,,,,,,,,,,,''''''''''''''''.....'''''.........''''''''..................  .                   ..;dOkxdl:,....  .,;,,,,,,,,,;;;:;,'.....','..............',,,''..'..''''''''''''''''''''..................                                                               
    .....''.'''..'''''''''''''''''''''',,,,,,,,'''''''''''''''''.......................''''''''...........................            .;xOkxdooc:,..    ...''''''''',,,'.... ..''...............,;;,,''''..............................  ..                                                                     
    ......'.'''..'''''''''''''''''''''''''''''''''''''''''..............................'.............................................:xkxxdddoll:'....     .'''..''......   ...........   ....',;;,''''..........................                                                                              
    ..........'''''''''''''''''''''''''''''''''''''''''''''.................................'........................................:xkxxdddddoolc;;::;.    ..''........     ......         ..',,,'''..........................                                                                                
    ...........'''''''''''''''''''''''''''''''''''''''''''''.............................''''..''...................................:xOkxxxxxxxxxdoc::loc.      ..........                   ..'','''............     ..                                                                                        
    .............'''''''''''''''''''''''''''''''''''''''''...............................'''...''.................................ckOkxdxxxxxxkkkxdlccll:,.        ......                   ...'''.........                                                                                                    
    ..............''''''''''''''''''''''''''''''''''''''''''''..................................................................'ckOkxdoddxxxxxxkkxolc:c::;'....    ......                  ....'.........                                                                                                     
    .............''''''''''''''''''''''''''''''''''''''.'..''...............................................................'',lkOkxdolloddxxxddxxdolc:::;;,,,'..    .....                 ...........                                                                                                        
    ..............''''''''''''''''''''''''''''''''''''....'''................................................................;okOOkxdolccllooodddoolcc:::;;,'''....   .....                 .........                                                                                                         
    ..............''''''''''''''''''''''''''''''''''''..''..................................................................;oO0Okkkxdolc::clllllllc:;;;;,,,''...'..........                 ......                                                                                                           
    ..................'''''''''''''''''''''''''''''''....................................................................'cxO0OOOkkxddolcc:ccc:::::;;;,,,,'''..................             ...                                                                                                              
    ....................'''''''''''''''''''''''''''''..................................................................';oO000OOOOkxddollcccc:;;;,,,,,'''''............  .......                                                                                                                             
    .....................''''''''''''''''''''''''''''.................................................................,lkO000000000Oxddollccc:;,,''''..........  .....   ........                                                                                                                            
    ......................'''''''''''''''''''''''''................................................................':dOOOO000000KK0Okxdoolcc:;,,''...........          ...,,,'...                                                                                                                           
    ............................'''''''''''''''''''................................................................:xOOOO000000000000Okxdolc::;,,''.........            .',;,,'...                                                                                                                          
     .............................'''''''''''''''''...............................................................;dOOOOO000000000000OOkxdolc::;,'''........             .',,,,...                                                                                                                          
    .    ............................'''''''''''''''.................................................................;okOOOO0000000000OOOOOkxdoolc:;,,''........             ..',,'...                                                                                                                          
    .    .............................''''''''''''..................................................................'cxkkkOOO00000000OOOkkkxddoolc::;,,''........            ..','...                                                                                                                           
    .    ................................'''''''''..................................................................,ldxxkkkOOOO000OOOkkkxddoollcc::;,,''.........           ..''.....                                                                                                                          
    .    ..................................'''.'''..................................................................,coddddxxkkkOOOkkxxddoollcc:::;;;,,''...........        ...'......                                                                                                                          
    ..   ...........................................................................................................':loooodddxxxxxdddoollccc:;;;;;,,,''..............    ...........                                                                                                                           
    .     ...........................................................................................................,cllllllloooooooolc:::;;;,,,,,''''.............................                                                                                                                            
    .     ...........................................................................................................';:cccccccllllllc:;;;,,,,''''''''.........    ... ............                                                                                                                             
    .     .............................................................................................................,:ccccccc:::::;,,,,,''.............           ....   ......                                                                                                                              
    ..     ..................................................................................''.........................,;::::::;;;,,,'''''......                     .         .                                                                                                                               
    ...    ..................................................................................''...........................',;::;;;,,''........                                                                                                                                                                  
    ....   ..............................................................................''''.''''..........................';::;,,,,'.......                                                                                                                                                                   
    .....  ..............................................................................'....''''............................';;;,,,'.......                                                                                                                                                                   
    ........................................................''',,;;;,,;;;;,,,''.........''''''..................................'',,,''.......                                                                                                                                                                  
    ...............................................''',,:clloddxxxxxxxxxxxxdddolc:;;,,''''''''....................................':c;,'........                                                                                                                                                                
    ............................................'',;cldxxkkkkkkkkkkkkkkkkkkkkkkkkxxxdolc:,'''....................................,lxxo:,,,'.............             .....                                                                                                                                      
    ........................................'',;cldxkkkkkkkkkkkxxxxxxkkkkkkkkkkkkkkkkkxxdol:;;,,''.............................':dOOkxo:;;,,'''...............................                                                                                                                                  
    .....................................'',:ldxkkkkkxxxxxxkkkkkxxxkkkkkkkkkkkkkkkkkkkkkkkkkxxddoc;,'.........................;okOOkkxolc:;;,,,''''......................................                                                                                                                       
    ..................................'',:loxkkkkxxxxxxxxxkkkkkkkkkkkkkkkkkkkkkkkkOOOOkkkkkkkkkkkkxoc;''....................':xOOkkkxxdolc:;;,,'''''''........................................                                                                                                                  
    ..............................',,:lodxxkkxxxxxxxxxxxxkkkkkkkkkkkkkkkkkkkkkkkkkkOOOkkkkkkkkkkkkkkxdl:,''................,lkOOkkkxxxddoc:;;,,,''''''''''''''.................................                                                                                                                 
    ............................';coxkkkkxxxxxxxxxxxxxxxxkkkkkkkkkkkkkkkkkkkkkkkkkkkOOkkkkkkkkxxxxxxkkkxdoc;,'...........';okOkkkkxxdddolc:;;,,,'''''....'..........................................                                                                                                            
               ......  . ....';:::;;::;;;;;;;;;::::;::::::::::::::::::::::::::::::::::::;;;;;;;;;:::::;'..............,:::::;;;;;,,,''..........................                                                                                                                                            
       .....   .... .............    ....    .....  ....   .....  .............    ....       .......   ....... .......   ...... ....  ...  ......    .......  ...  ...    ....   ....  ...    .......   .......   ....     ...    ....   ................    .....  .............    ....    ...   ..      
      :0KKKKl .oKK0ooKK0kOKKKKKKKo,oOKKK0x,.lKKKKd,oKKKKc :0KKKKlc0KKKKKK0kOKKx..lO0KK0kc.   'kKKKKK0k:.oKKKKKxcxKKKKK0x,l0KKKKOldKKk,c0Kk,cKKKKKK0o.,dkKKKKK:;0K0:,kKKc.;x0KKK0d,c0KO:c0KO;  .xKKKKKKk;.lKKKKKk,,x0KKK0d' cKKK:'d0KKK0k:cOKKKKKKKOOKKKKK0k; 'kKKKKdckKKKKKKKOkKK0;.:k0KKKOc.;OKKl.oKKl     
      dWWWMMk..xMMWdxWMNxlxXMMWOolOWMNOKMM0;oWMMMKd0MMMMo.oMWWWMk:lkNMMNkloKWMO,oWMWOOWMWo   ,KMWXOKWMKcxMMW0oc:OMMNOKMWddWMMKdl:kMMWkdNMK;oMMWO0WMWl;kKMMXxo'cNMM0o0MWocXMM0kXMWk:OMWO0MWk.  .OMMNOKMMO,dMMW0dc:0MWXOKWM0'lMMWdxWMNk0MMKolxXMMWOodKMMXOKMMO.;XMWWMXocdKMMWOoo0MMN::XMM00WMWocXMMKlkMMd.    
     .OMWXNMK,.xMMWdxMMWc .kMMNc '0MMKokMMXcdWMMMWXNMMMMo.kMWXNMK, ,KMMK, '0MMO;xMMNooNNNx.  ,KMM0oOMMXlkMMWd'.,0MMXdOMWddWMMk,.'kMMMNKNMK;oMMNodWMMo;xKMMO;..cNMMWXXMWooNMMkc0NNO,cNMWWMX;   .OMMXk0MMO,dMMWd'.:XMM0coOOx,oMMWddWMWKOxc;. .kMMN: '0MMXx0MWk'lWMNXMWo .xMMWl .OMMNccNMMxoNMMxlXMMWXXMMx.    
     ,KMNkKMWc.xMMWdxMMWc .kMMNc '0MMXokMMXldWMNNWMMNNMMo;KMNkKMWl ,KMMK, '0MMO;xMMWl.,,,.   ,KMM0oOMMXokMMMNXxcOMMWNWMNldWMMWXkckMMMMWMMK;oMMNodMMMo;xKMMWXK:cNMMMMMMWooWMMk'',,'..kMMMWo    .OMMWNWMNl.dMMMNXdoKMMKl:cc:.oWMWl.cONMMXx;  .kMMNc '0MMWNWMXl.xMM0OWMk..kMMWl .OMMNloWMMxoNMMxlXMMMMMMMx.    
     cNMNOKMMx'xMMWdxMMWc .kMMNc '0MMKokMMXldWMXKWMWKXMMdlNMNOKMMx.,KMMK, '0MMO;xMMNockOOl.  ,KMM0oOMMXokMMWx,.,0MMNxcc,.oWMMO;.'kMMNNMMMK;oMMNdxMMMo:xKMM0:'.cNMWNMMMWooNMMk:dOOd. ;XMM0'    .OMMXdOMMO,dMMWx,.:XMMKkXMMX:oMMWo';lk0XMMX: .kMMNc '0MMXd0MMkcOMW00WMK,.kMMWl .OMMNclWMMxoNMMxlXMWNWMMMd.    
     dMMWNNMMO;dMMWkOMMNc .kMMNc '0MMXx0MMK:dMMXOXMNOKMMkxWMWNNMM0',KMMK, '0WMO,dMMWxkWMWd   ,KMMKxKMMXlkMMWk;,;0MMX:    oWMMO:,;OMW0OWMMK;oMMWxkMMMo;xKMM0c;'lNMKkXMMWolNMMOdXMMO. '0MMk.    .OMMXokMM0;dMMWk;,cKMMKd0MMK;oMMWddWMNdkWMWo .kMMNc '0MMKoOMMOdKMMNNWMN:.xMMWl .OMMN:cNMMOkNMMxlXMXk0MMMd.    
    .OMMXokMMNc,kNWWWWXx. .kMMNc  :0WWWWWKl.dMMKdOMKd0MW00MMXokMMN:,KMMK; ,0WWO',ONWWWWXx.   ,KMMWWWMWk;kMMMMWKdOMMN:    dWMMMMWO0MMkc0MMK;oMMMWWMWX:;kKMMMMMkdNM0:dWMWo'dXWWWWNO;  '0MMO.    'OMMXokMM0;dMMMMWXlcKWWNNWMX:oWMWl,kNWNWMNk' .OMMNc ,0MMKoOMMK0NMWxdNMWd.xMMWo .OMMNc.dXWWWWNk,cNMNloWMMx.    
    .,;;,..;;;.  ';;;;.    ';;;.   .';;;,.  .;;,.';'.';;,,;;,..;;;..,;;,. .,;;'  .';;;,.     .,;;;;;;,. ';;;;;,.';;,.    .;;;;;;',;;'..;;,..;;;;;;;. .',;;;;;'.,;,..;;;.  .,;;;'.    ';;'      ';;,.';;'..;;;;;;. .,;;,,;,..;;;.  ';;;;'.   ';;;. .,;;,.';;,,;;;..,;;'..;;;.  ';;;.  .,;;;'  .;;;..,;;.     

          ...                                                                                                                                                                                                                                                                                               
    .:c;:;;,;ldxo:;,..,,;,;:;:;.                                                                                                                                                                                                                                                                                
    .;c;::oddl;cldl,.';,:;:::::.                                                                                                                                                                                                                                                                                
      .,'    ..                                       
         */
    }
}