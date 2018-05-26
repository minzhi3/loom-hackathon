using System;
using System.Threading.Tasks;
using UnityEngine;
using Loom.Unity3d;
using Loom.Unity3d.Samples;

public class LoomTools : MonoBehaviour
{
	public static LoomTools Instance;
	Contract contract;
    public Identity identity;
    public int userId;

    private void Awake()
    {
        if (Instance != null)
        {
			Debug.LogError("LoomTools failed");
        }
        else
        {
            Instance = this;
        }
    }

    async Task<Contract> GetContract(byte[] privateKey, byte[] publicKey)
    {
        var writer = RPCClientFactory.Configure()
            .WithLogger(Debug.unityLogger)
            .WithHTTP("http://192.168.251.50:46658/rpc")
            //.WithWebSocket("ws://127.0.0.1:46657/websocket")
            .Create();

        var reader = RPCClientFactory.Configure()
            .WithLogger(Debug.unityLogger)
            .WithHTTP("http://192.168.251.50:46658/query")
            //.WithWebSocket("ws://127.0.0.1:47000/queryws")
            .Create();

        var client = new DAppChainClient(writer, reader)
        {
            Logger = Debug.unityLogger
        };
        // required middleware
        client.TxMiddleware = new TxMiddleware(new ITxMiddlewareHandler[]{
            new NonceTxMiddleware{
                PublicKey = publicKey,
                Client = client
            },
            new SignedTxMiddleware(privateKey)
        });

        var contractAddr = await client.ResolveContractAddressAsync("BluePrint");
        var callerAddr = Address.FromPublicKey(publicKey);
        return new Contract(client, contractAddr, callerAddr);
    }

    async Task CallContract(Contract contract)
    {

        try
        {
            var result = await contract.StaticCallAsync<MapEntry>("GetMsg", new MapEntry
            {
                Key = "usercount"
            });

            userId = int.Parse(result.Value.ToString()) + 1;
            //var x = result.ToString();

            await contract.CallAsync("SetMsg", new MapEntry
            {
                Key = "usercount",
                Value = "userId"
            });


        }
        catch
        {
            await contract.CallAsync("SetMsg", new MapEntry
            {
                Key = "usercount",
                Value = "1"
            });
            userId = 1;
        }

        await contract.CallAsync("SetMsg", new MapEntry
        {
            Key = "coin" + userId.ToString(),
            Value = "0"
        });

        Debug.Log("Now Your user ID is: " + userId.ToString());

    }

    async Task CallContractWithResult(Contract contract)
    {
        var result = await contract.CallAsync<MapEntry>("SetMsgEcho", new MapEntry
        {
            Key = "321",
            Value = "456"
        });

        if (result != null)
        {
            // This should print: { "key": "321", "value": "456" } in the Unity console window.
            Debug.Log("Smart contract returned: " + result.ToString());
        }
        else
        {
            throw new Exception("Smart contract didn't return anything!");
        }
    }

    async Task StaticCallContract(Contract contract)
    {
        var result = await contract.StaticCallAsync<MapEntry>("GetMsg", new MapEntry
        {
            Key = "123"
        });

        if (result != null)
        {
            // This should print: { "key": "123", "value": "hello!" } in the Unity console window
            // provided `LoomQuickStartSample.CallContract()` was called first.
            Debug.Log("Smart contract returned: " + result.ToString());
        }
        else
        {
            throw new Exception("Smart contract didn't return anything!");
        }
    }

    
    public async Task<String> GetCoinAmount()
    {

        var result = await contract.StaticCallAsync<MapEntry>("GetMsg", new MapEntry
        {
            Key = "coin" + userId.ToString()
        });

        if (result != null)
        {
            return result.Value.ToString();

        }
        else
        {
            throw new Exception("Smart contract didn't return anything!");
        }
    }

    public async Task Ranking()
    {
        var str = await GetCoinAmount();
        

    }



    public async Task GetCoin()
    {
        var result = await contract.StaticCallAsync<MapEntry>("GetMsg", new MapEntry
        {
            Key = "coin" + userId.ToString()
        });

        if (result != null)
        {
            // This should print: { "key": "123", "value": "hello!" } in the Unity console window
            // provided `LoomQuickStartSample.CallContract()` was called first.
            var x = int.Parse(result.Value.ToString()) + 1;
            //var x = result.ToString();
            Debug.Log("You got coin and now your coin is : " + x);

            await contract.CallAsync("SetMsg", new MapEntry
            {
                Key = "coin" + userId.ToString(),
                Value = x.ToString(),
            });

        }
        else
        {
            throw new Exception("Smart contract didn't return anything!");
        }
    }

    public async Task<bool> Purchase(int _price)
    {

        var result = await contract.StaticCallAsync<MapEntry>("GetMsg", new MapEntry
        {
            Key = "coin" + userId.ToString()
        });

        if (result != null)
        {

            // This should print: { "key": "123", "value": "hello!" } in the Unity console window
            // provided `LoomQuickStartSample.CallContract()` was called first.
            var x = int.Parse(result.Value.ToString());

            if (_price < x)
            {

                x = x - _price;
                //var x = result.ToString();
                Debug.Log("You purchased new character and now your coin is : " + x);

                await contract.CallAsync("SetMsg", new MapEntry
                {
                    Key = "coin" + userId.ToString(),
                    Value = x.ToString(),
                });

                return true;
            }
            else
            {

                return false;

            }

        }
        else
        {
            throw new Exception("Smart contract didn't return anything!");
        }
    }


    public async Task CreateAccount()
    {

    }






    // Use this for initialization
    async void Start()
    {
        // The private key is used to sign transactions sent to the DAppChain.
        // Usually you'd generate one private key per player, or let them provide their own.
        // In this sample we just generate a new key every time.
        var privateKey = CryptoUtils.GeneratePrivateKey();
        var publicKey = CryptoUtils.PublicKeyFromPrivateKey(privateKey);

		contract = await GetContract(privateKey, publicKey);

        await CallContract(contract);

        // This should print: { "key": "123", "value": "hello!" } in the Unity console window
        //       await StaticCallContract(contract);
        // This should print: { "key": "321", "value": "456" } in the Unity console window
        //        await CallContractWithResult(contract);
    }
}
