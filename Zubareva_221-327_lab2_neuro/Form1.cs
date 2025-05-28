using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;
using static LyachovNeuroLab2.Form1;
using static System.Windows.Forms.LinkLabel;
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;

namespace LyachovNeuroLab2
{
    public partial class Form1 : Form
    {
        


        public class NeuralNetwork
        {
            public int inputSize, hiddenSize, outputSize;
            public float[,] weightsInputHidden;
            public float[,] weightsHiddenOutput;
            public Random rand = new Random();
            

            public NeuralNetwork(int inputSize, int hiddenSize, int outputSize)
            {
                this.inputSize = inputSize;
                this.hiddenSize = hiddenSize;
                this.outputSize = outputSize;

                weightsInputHidden = new float[inputSize, hiddenSize];
                weightsHiddenOutput = new float[hiddenSize, outputSize];

                InitializeWeights();

            }
            public void TrainOnDataset(List<(float[] input, int correctAction)> dataset, int epochs = 1000)
            {
                for (int e = 0; e < epochs; e++)
                {
                    foreach (var (input, action) in dataset)
                    {
                        Train(input, action);
                    }
                }
            }

            private void InitializeWeights()
            {
                for (int i = 0; i < inputSize; i++)
                    for (int j = 0; j < hiddenSize; j++)
                        weightsInputHidden[i, j] = (float)(rand.NextDouble() * 2 - 1);

                for (int i = 0; i < hiddenSize; i++)
                    for (int j = 0; j < outputSize; j++)
                        weightsHiddenOutput[i, j] = (float)(rand.NextDouble() * 2 - 1);
            }

            private float[] ReLU(float[] x)
            {
                float[] output = new float[x.Length];
                for (int i = 0; i < x.Length; i++)
                    output[i] = Math.Max(0, x[i]);
                return output;
            }

            private float[] Softmax(float[] x)
            {
                float sum = 0;
                float[] output = new float[x.Length];

                for (int i = 0; i < x.Length; i++)
                    sum += (float)Math.Exp(x[i]);

                for (int i = 0; i < x.Length; i++)
                    output[i] = (float)Math.Exp(x[i]) / sum;

                return output;
            }

            private float[] MatrixMultiply(float[] input, float[,] weights)
            {
                int rows = weights.GetLength(0);
                int cols = weights.GetLength(1);
                float[] output = new float[cols];

                for (int j = 0; j < cols; j++)
                {
                    for (int i = 0; i < rows; i++)
                        output[j] += input[i] * weights[i, j];
                }

                return output;
            }

            public int Predict(float[] input)
            {
                float[] hiddenLayer = ReLU(MatrixMultiply(input, weightsInputHidden));
                float[] outputLayer = Softmax(MatrixMultiply(hiddenLayer, weightsHiddenOutput));

                int bestAction = 0;
                float maxVal = outputLayer[0];

                for (int i = 1; i < outputLayer.Length; i++)
                {
                    if (outputLayer[i] > maxVal)
                    {
                        maxVal = outputLayer[i];
                        bestAction = i;
                    }
                }

                return bestAction;
            }

            public string SendCommand(int action)
            {
                string command = "FORWARD"; // Значение по умолчанию

                if (action == 0)
                    command = "LEFT";
                else if (action == 1)
                    command = "FORWARD";
                else if (action == 2)
                    command = "RIGHT";
                return command;
            }


            public void SaveWeights(string filePath)
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int i = 0; i < inputSize; i++)
                    {
                        for (int j = 0; j < hiddenSize; j++)
                        {
                            writer.Write(weightsInputHidden[i, j] + " ");
                        }
                        writer.WriteLine();
                    }

                    writer.WriteLine("----");

                    for (int i = 0; i < hiddenSize; i++)
                    {
                        for (int j = 0; j < outputSize; j++)
                        {
                            writer.Write(weightsHiddenOutput[i, j] + " ");
                        }
                        writer.WriteLine();
                    }
                }
            }


            public (int action, float[] probabilities) PredictWithConfidence(float[] input)
            {
                float[] hiddenLayer = ReLU(MatrixMultiply(input, weightsInputHidden));
                float[] outputLayer = Softmax(MatrixMultiply(hiddenLayer, weightsHiddenOutput));

                int bestAction = 0;
                float maxVal = outputLayer[0];

                for (int i = 1; i < outputLayer.Length; i++)
                {
                    if (outputLayer[i] > maxVal)
                    {
                        maxVal = outputLayer[i];
                        bestAction = i;
                    }
                }

                return (bestAction, outputLayer);
            }

            public void LoadWeights(string filePath)
            {
                if (!System.IO.File.Exists(filePath))
                {
                    Console.WriteLine("Файл весов не найден. Используются случайные значения.");
                    return;
                }

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    int i = 0, j = 0;
                    bool readingHiddenLayer = true;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == "----")
                        {
                            readingHiddenLayer = false;
                            i = 0;
                            continue;
                        }

                        string[] values = line.Split(' ');

                        if (readingHiddenLayer)
                        {
                            for (j = 0; j < hiddenSize; j++)
                                weightsInputHidden[i, j] = float.Parse(values[j]);
                        }
                        else
                        {
                            for (j = 0; j < outputSize; j++)
                                weightsHiddenOutput[i, j] = float.Parse(values[j]);
                        }
                        i++;
                    }
                }
            }

            public float[] GetWeights()
            {
                int weightCount = (inputSize * hiddenSize) + (hiddenSize * outputSize);
                float[] weights = new float[weightCount];

                int index = 0;

                // Копируем веса между входом и скрытым слоем
                for (int i = 0; i < inputSize; i++)
                {
                    for (int j = 0; j < hiddenSize; j++)
                    {
                        weights[index++] = weightsInputHidden[i, j];
                    }
                }

                // Копируем веса между скрытым слоем и выходом
                for (int i = 0; i < hiddenSize; i++)
                {
                    for (int j = 0; j < outputSize; j++)
                    {
                        weights[index++] = weightsHiddenOutput[i, j];
                    }
                }

                return weights;
            }

            public void SetWeights(float[] weights)
            {
                int index = 0;

                // Устанавливаем веса между входом и скрытым слоем
                for (int i = 0; i < inputSize; i++)
                {
                    for (int j = 0; j < hiddenSize; j++)
                    {
                        weightsInputHidden[i, j] = weights[index++];
                    }
                }

                // Устанавливаем веса между скрытым слоем и выходом
                for (int i = 0; i < hiddenSize; i++)
                {
                    for (int j = 0; j < outputSize; j++)
                    {
                        weightsHiddenOutput[i, j] = weights[index++];
                    }
                }
            }

            public void Train(float[] input, int correctAction, float learningRate = 0.01f)
            {
                // Прямое распространение
                float[] hiddenInput = MatrixMultiply(input, weightsInputHidden);
                float[] hiddenOutput = ReLU(hiddenInput);

                float[] finalInput = MatrixMultiply(hiddenOutput, weightsHiddenOutput);
                float[] finalOutput = Softmax(finalInput);

                // Ошибка на выходе
                float[] target = new float[outputSize];
                target[correctAction] = 1; // только правильный класс = 1

                float[] outputErrors = new float[outputSize];
                for (int i = 0; i < outputSize; i++)
                    outputErrors[i] = target[i] - finalOutput[i];

                // Обновление весов: hidden → output
                for (int i = 0; i < hiddenSize; i++)
                {
                    for (int j = 0; j < outputSize; j++)
                    {
                        weightsHiddenOutput[i, j] += learningRate * outputErrors[j] * hiddenOutput[i];
                    }
                }

                // Ошибка на скрытом слое
                float[] hiddenErrors = new float[hiddenSize];
                for (int i = 0; i < hiddenSize; i++)
                {
                    float error = 0;
                    for (int j = 0; j < outputSize; j++)
                    {
                        error += outputErrors[j] * weightsHiddenOutput[i, j];
                    }

                    // Применяем производную ReLU
                    hiddenErrors[i] = hiddenInput[i] > 0 ? error : 0;
                }

                // Обновление весов: input → hidden
                for (int i = 0; i < inputSize; i++)
                {
                    for (int j = 0; j < hiddenSize; j++)
                    {
                        weightsInputHidden[i, j] += learningRate * hiddenErrors[j] * input[i];
                    }
                }
            }
        }

        public class Genome
        {
            public NeuralNetwork Network { get; set; }
            public double Fitness { get; set; }
        }

        public List<Genome> SelectTop(List<Genome> population, int count)
        {
            return population.OrderByDescending(g => g.Fitness).Take(count).ToList();
        }

        // Метод для создания начальной популяции нейросетей
        private List<Genome> CreateInitialPopulation(int size, int inputSize, int hiddenSize, int outputSize)
        {
            var population = new List<Genome>();
            for (int i = 0; i < size; i++)
            {
                var net = new NeuralNetwork(inputSize, hiddenSize, outputSize);
                population.Add(new Genome { Network = net });
            }
            return population;
        }

        // Метод для оценки фитнеса нейросетей в популяции
        private void EvaluateFitness(List<Genome> population, List<(float[] input, int correctAction)> dataset)
        {
            foreach (var genome in population)
            {
                genome.Fitness = TestNetwork(genome.Network, dataset);
            }
        }

        // Метод для тестирования нейросети и расчёта её фитнеса
        private double TestNetwork(NeuralNetwork network, List<(float[] input, int correctAction)> dataset)
        {
            double totalFitness = 0;
            foreach (var (input, correctAction) in dataset)
            {
                int predictedAction = network.Predict(input);
                if (predictedAction == correctAction)
                {
                    totalFitness += 1;
                }
            }
            return totalFitness;
        }

        // Метод для выбора лучших нейросетей в популяции



        // Метод для кроссовера двух нейросетей (смешивание их весов)
        // Метод для кроссовера двух нейросетей (смешивание их весов)
        // Метод для кроссовера двух нейросетей
        private NeuralNetwork Crossover(NeuralNetwork parent1, NeuralNetwork parent2)
        {
            var child = new NeuralNetwork(parent1.inputSize, parent1.hiddenSize, parent1.outputSize);

            var weights1 = parent1.GetWeights();
            var weights2 = parent2.GetWeights();

            var newWeights = new float[weights1.Length];
            var rand = new Random();

            for (int i = 0; i < newWeights.Length; i++)
            {
                newWeights[i] = rand.NextDouble() < 0.5 ? weights1[i] : weights2[i];
            }

            child.SetWeights(newWeights);
            return child;
        }

        // Метод для мутации нейросети
        private void Mutate(NeuralNetwork net, double mutationRate = 0.05, double mutationAmount = 0.1)
        {
            var rand = new Random();
            var weights = net.GetWeights();

            for (int i = 0; i < weights.Length; i++)
            {
                if (rand.NextDouble() < mutationRate)
                {
                    weights[i] += (float)((rand.NextDouble() * 2 - 1) * mutationAmount);
                }
            }

            net.SetWeights(weights);
        }

        // Метод для создания нового поколения на основе старого
        private List<Genome> NextGeneration(List<Genome> current)
        {
            var next = new List<Genome>();
            var top3 = SelectTop(current, 3);
            next.AddRange(top3);

            var best5 = SelectTop(current, 5);

            var rand = new Random();

            for (int i = 0; i < 5; i++)
            {
                var parent1 = best5[rand.Next(best5.Count)];
                var parent2 = best5[rand.Next(best5.Count)];
                var childNet = Crossover(parent1.Network, parent2.Network);
                Mutate(childNet);
                next.Add(new Genome { Network = childNet });
            }

            for (int i = 0; i < 2; i++)
            {
                var net = new NeuralNetwork(current[0].Network.inputSize, current[0].Network.hiddenSize, current[0].Network.outputSize);
                net.GetWeights();
                next.Add(new Genome { Network = net });
            }

            return next;
        }



        public Form1()
        {
            InitializeComponent();
            nn = new NeuralNetwork(8, 10, 3);
            nn.TrainOnDataset(dataset, epochs: 100);
            RunEvolution(generations: 50, populationSize: 30, dataset);
        }
        IPAddress ipAddress;
        public static int port, received, sec = 0;
        byte[] data;
        NeuralNetwork nn;
        private float[] NormalizeDistances(float d0, float d1, float d2, float d3, float d4, float d5, float d6, float d7, float maxDistance = 1500f)
        {
            return new float[]
            {
                d0 / maxDistance,
                d1 / maxDistance,
                d2 / maxDistance,
                d3 / maxDistance,
                d4 / maxDistance,
                d5 / maxDistance,
                d6 / maxDistance,
                d7 / maxDistance
            };
        }
        private void RunEvolution(int generations, int populationSize, List<(float[] input, int correctAction)> dataset)
        {
            int inputSize = dataset[0].input.Length;
            int hiddenSize = 16; // по желанию
            int outputSize = 3;

            var population = CreateInitialPopulation(populationSize, inputSize, hiddenSize, outputSize);

            for (int gen = 0; gen < generations; gen++)
            {
                EvaluateFitness(population, dataset);
                var bestFitness = population.Max(g => g.Fitness);
                Console.WriteLine($"Поколение {gen + 1}: Лучшая пригодность = {bestFitness}");

                // (опционально) отобрази результат или сохрани лучшую сеть

                population = NextGeneration(population);
            }

            var best = SelectTop(population, 1).First();
            best.Network.SaveWeights("best_weights.txt");
        }
        private void TestBestNetwork(List<(float[] input, int correctAction)> dataset)
        {
            var nn = new NeuralNetwork(dataset[0].input.Length, 16, 3);
            nn.LoadWeights("best_weights.txt");

            double fitness = TestNetwork(nn, dataset);
            MessageBox.Show($"Точность сети: {fitness}/{dataset.Count} ({(fitness / dataset.Count * 100):F2}%)");
        }

        float score = 0.0f;
        float previousDistance = -1f;
        bool isStopped = false;
        float stopThreshold = -10f;


        float goalX = 15.15f;
        float goalY = 9.15f;

        float maxDistance = 20f; // максимально возможное расстояние в лабиринте
        float timePenaltyFactor = 0.1f;

        float startTime; // для подсчёта времени

        //private float Distance(float x1, float y1, float x2, float y2)
        //{
        //    return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        //}



        List<(float[] input, int correctAction)> dataset = new List<(float[], int)>
        {
            //(new float[] {0.1f, 0.2f, 0.4f, 0.9f, 0.7f, 0.6f, 0.2f, 0.1f}, 1),
            (new float[] {0.1f, 0.2f, 0.4f, 0.9f, 0.7f, 0.6f, 0.2f, 0.9f}, 1),
            (new float[] {0.1f, 0.2f, 0.7f, 0.9f, 0.7f, 0.3f, 0.2f, 0.1f}, 1),
            (new float[] {0.1f, 0.1f, 0.9f, 0.9f, 0.9f, 0.2f, 0.2f, 0.1f}, 1),
            (new float[] {0.2f, 0.3f, 0.2f, 0.1f, 0.1f, 0.6f, 0.8f, 0.9f}, 2),
            (new float[] {0.2f, 0.3f, 0.1f, 0.1f, 0.7f, 0.8f, 0.8f, 0.9f}, 2),
            (new float[] {0.2f, 0.3f, 0.2f, 0.1f, 0.1f, 0.9f, 0.9f, 0.9f}, 2),
            (new float[] {0.1f, 0.1f, 0.1f, 0.1f, 0.9f, 0.9f, 0.9f, 0.9f}, 2),
            (new float[] {0.9f, 0.8f, 0.7f, 0.1f, 0.2f, 0.2f, 0.1f, 0.1f}, 0),
            (new float[] {0.9f, 0.8f, 0.5f, 0.1f, 0.1f, 0.2f, 0.3f, 0.2f}, 0),
            (new float[] {0.9f, 0.5f, 0.5f, 0.1f, 0.1f, 0.2f, 0.3f, 0.2f}, 0),
            (new float[] {0.9f, 0.8f, 0.9f, 0.9f, 0.1f, 0.1f, 0.1f, 0.1f}, 0),

        };

        IPEndPoint iPEndPoint;
        Thread thread;
        public static float n, s, c, le, re, az, b, d0, d1, d2, d3, d4, d5, d6, d7, l0, l1, l2, l3, l4, sp, X, Y, T;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtnTrainNetwork_Click(object sender, EventArgs e)
        {
            int[,] map = LoadMapToArray("labyrinth.txt");
            TestBestNetwork(dataset);
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            IPEndPoint pointServer = new IPEndPoint(ipAddress, 8888);
            decodeTextstr["TEXT"] = "RESTART";

            string oldCommands = JsonConvert.SerializeObject(decodeTextstr, Formatting.None);
            byte[] data = Encoding.ASCII.GetBytes(oldCommands + "\n");
            udpCommands.Send(data, data.Length, pointServer);

            commands["T"] = 1; // Отправка команды "RESTART"
            string restartCommand = JsonConvert.SerializeObject(commands, Formatting.None);
            byte[] restartData = Encoding.ASCII.GetBytes(restartCommand + "\n");
            udpCommands.Send(restartData, restartData.Length, pointServer);

            while (sp != 1)
            {
                // Ждем, пока робот не окажется в стартовой позиции
                Thread.Sleep(100);
                DecodingData(data); // Обновляем данные от робота
            }

            InformTextbox.Text += "\r\n[INFO] Робот находится в стартовой позиции.";
        }

        UdpClient udpClient;
        UdpClient udpCommands = new UdpClient();
        Dictionary<string, float> decodeText;
        Dictionary<string, string> decodeTextstr = new Dictionary<string, string>();

        public static Dictionary<string, int> commands = new Dictionary<string, int>
        {
            { "N", 0 },
            { "M", 0 },
            { "F", 0 },
            { "B", 0 },
            { "T", 0 },
        };

        public static string jsonString, jsonString2, message;

        private void DecodingData(byte[] data)
        {
            var message = Encoding.ASCII.GetString(data);
            decodeText = JsonConvert.DeserializeObject<Dictionary<string, float>>(message);
            var lines = decodeText.Select(kv => kv.Key + ": " + kv.Value.ToString());
            textBoxData.Text = "IoT: " + string.Join(Environment.NewLine, lines);

            AnalyzeData(decodeText);
        }

        private void AnalyzeData(Dictionary<string, float> pairs)
        {
            if (pairs.ContainsKey("n"))
            {
                n = pairs["n"];
                s = pairs["s"];
                c = pairs["c"];
                le = pairs["le"];
                re = pairs["re"];
                az = pairs["az"];
                b = pairs["b"];
                d0 = pairs["d0"];
                d1 = pairs["d1"];
                d2 = pairs["d2"];
                d3 = pairs["d3"];
                d4 = pairs["d4"];
                d5 = pairs["d5"];
                d6 = pairs["d6"];
                d7 = pairs["d7"];
                l0 = pairs["l0"];
                l1 = pairs["l1"];
                l2 = pairs["l2"];
                l3 = pairs["l3"];
                l4 = pairs["l4"];
                sp = pairs["sp"];
                X = pairs["x"];
                Y = pairs["y"];
                T = pairs["t"];
            }
            else
            {
                MessageBox.Show("No data");
            }
        }

        private void Receive()
        {
            while (true)
            {
                try
                {
                    data = udpClient.Receive(ref iPEndPoint);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //private void Send()
        //{
        //    IPEndPoint pointServer = new IPEndPoint(ipAddress, 8888);


        //    InformTextbox.Text = message;

        //    SerializeCommands(commands);
        //    decodeText["n"] = commands["N"];


        //    string oldCommands = JsonConvert.SerializeObject(decodeText, Formatting.None);
        //    byte[] data = Encoding.ASCII.GetBytes(oldCommands + "\n");
        //    udpCommands.Send(data, data.Length, pointServer);

        //    byte[] dataForRobot = Encoding.ASCII.GetBytes(jsonString + "\n");
        //    udpCommands.Send(dataForRobot, dataForRobot.Length, pointServer);

        //    float[] inputs = NormalizeDistances(d0, d1, d2, d3, d4, d5, d6, d7);


        //    // Получаем предсказание и вероятности
        //    var (action, probabilities) = nn.PredictWithConfidence(inputs);
        //    string command = nn.SendCommand(action);

        //    // Логируем вероятности и решение
        //    InformTextbox.Text += $"\r\n[AI] Command: {command} ({action})";
        //    InformTextbox.Text += $"\r\n[AI] Confidence: LEFT={probabilities[0]:0.00}, FORWARD={probabilities[1]:0.00}, RIGHT={probabilities[2]:0.00}";

        //    Robot.Moving(command);

        //    // Автообучение — опционально, можно обучить на "разумной" логике
        //    int correctAction = DetermineCorrectAction(inputs); // <- ты можешь сам написать этот метод!
        //    nn.Train(inputs, correctAction);  // обучение на лету

        //    float distanceToGoal = Distance(X, Y, goalX, goalY);
        //    float timeSpent = (Environment.TickCount / 1000f) - startTime;

        //    float fitness = maxDistance - distanceToGoal - (timePenaltyFactor * timeSpent);

        //    InformTextbox.Text += $"\r\n[AI] Fitness: {fitness:0.00}";
        //    // Робот двигается
        //    if (isStopped)
        //    {
        //        InformTextbox.Text += "\r\n[STOPPED] Робот остановлен. Нажми Start заново.";
        //        return;
        //    }

        //    float currentDistance = Distance(X, Y, goalX, goalY);
        //    float timeNow = Environment.TickCount / 1000f;
        //    float deltaTime = 1f / 2f; // таймер тикает примерно 2 раза в секунду

        //    // Проверка на столкновение
        //    if (b == 1)
        //    {
        //        isStopped = true;
        //        InformTextbox.Text += "\r\n[ALERT] Столкновение! Нажми Start, чтобы попробовать заново.";
        //        decodeTextstr["TEXT"] = "RESTART";
        //        return;
        //    }

        //    // Первая итерация
        //    if (previousDistance < 0)
        //        previousDistance = currentDistance;

        //    // Вычисляем награду
        //    float reward = CalculateReward(currentDistance, timeSpent, deltaTime, inputs);
        //    score += reward;

        //    InformTextbox.Text += $"\r\n[REWARD] ΔScore: {reward:0.00}, Total: {score:0.00}";

        //    // Сохраняем дистанцию на след. тик
        //    previousDistance = currentDistance;

        //    // Проверка на провал
        //    if (score < stopThreshold)
        //    {
        //        isStopped = true;
        //        textBoxData.Text += "\r\n[FAIL] Робот слишком долго не может пройти. Очки в минусе.";
        //        decodeTextstr["TEXT"] = "RESTART";
        //        return;
        //    }
        //}
        private void Send()
        {
            IPEndPoint pointServer = new IPEndPoint(ipAddress, 8888);

            InformTextbox.Text = message;

            SerializeCommands(commands);
            decodeText["n"] = commands["N"];

            string oldCommands = JsonConvert.SerializeObject(decodeText, Formatting.None);
            byte[] data = Encoding.ASCII.GetBytes(oldCommands + "\n");
            udpCommands.Send(data, data.Length, pointServer);

            byte[] dataForRobot = Encoding.ASCII.GetBytes(jsonString + "\n");
            udpCommands.Send(dataForRobot, dataForRobot.Length, pointServer);

            float[] inputs = NormalizeDistances(d0, d1, d2, d3, d4, d5, d6, d7);

            // Получаем предсказание и вероятности
            var (action, probabilities) = nn.PredictWithConfidence(inputs);
            string command = nn.SendCommand(action);

            // Логируем вероятности и решение
            InformTextbox.Text += $"\r\n[AI] Command: {command} ({action})";
            InformTextbox.Text += $"\r\n[AI] Confidence: LEFT={probabilities[0]:0.00}, FORWARD={probabilities[1]:0.00}, RIGHT={probabilities[2]:0.00}";

            Robot.Moving(command);

            // Автообучение — опционально, можно обучить на "разумной" логике
            int correctAction = DetermineCorrectAction(inputs); // <- ты можешь сам написать этот метод!
            nn.Train(inputs, correctAction);  // обучение на лету

            float distanceToGoal = Distance(X, Y, goalX, goalY);
            float timeSpent = (Environment.TickCount / 1000f) - startTime;

            float fitness = maxDistance - distanceToGoal - (timePenaltyFactor * timeSpent);

            InformTextbox.Text += $"\r\n[AI] Fitness: {fitness:0.00}";
            // Робот двигается
            if (isStopped)
            {
                InformTextbox.Text += "\r\n[STOPPED] Робот остановлен. Нажми Start заново.";
                return;
            }

            float currentDistance = Distance(X, Y, goalX, goalY);
            float timeNow = Environment.TickCount / 1000f;
            float deltaTime = 1f / 2f; // таймер тикает примерно 2 раза в секунду

            // Проверка на столкновение
            if (b == 1)
            {
                isStopped = true;
                InformTextbox.Text += "\r\n[ALERT] Столкновение! Нажми Start, чтобы попробовать заново.";
                decodeTextstr["TEXT"] = "RESTART";
                RestartRobot();

                return;
            }

            // Первая итерация
            if (previousDistance < 0)
                previousDistance = currentDistance;

            // Вычисляем награду
            float reward = CalculateReward(currentDistance, timeSpent, deltaTime, inputs);
            score += reward;

            InformTextbox.Text += $"\r\n[REWARD] ΔScore: {reward:0.00}, Total: {score:0.00}";

            // Сохраняем дистанцию на след. тик
            previousDistance = currentDistance;

            // Проверка на провал
            if (score < -2) // Если score уходит в сильный минус, отправляем команду "RESTART"
            {
                isStopped = true;
                InformTextbox.Text += "\r\n[FAIL] Робот слишком долго не может пройти. Очки в минусе.";
                decodeTextstr["TEXT"] = "RESTART";
                fitness = 0;
                score = 0;

                RestartRobot();
                return;
            }
        }
        private Dictionary<string, string> textCommands = new Dictionary<string, string>();

        private void RestartRobot()
        {
            IPEndPoint pointServer = new IPEndPoint(ipAddress, 8888);
            textCommands["TEXT"] = "RESTART"; // Отправка команды "RESTART"
            string restartCommand = JsonConvert.SerializeObject(textCommands, Formatting.None);
            byte[] restartData = Encoding.ASCII.GetBytes(restartCommand + "\n");
            udpCommands.Send(restartData, restartData.Length, pointServer);

            while (sp != 1)
            {
                // Ждем, пока робот не окажется в стартовой позиции
                Thread.Sleep(100);
                DecodingData(data); // Обновляем данные от робота
            }

            InformTextbox.Text += "\r\n[INFO] Робот перезапущен.";
            score = 0; // Сбрасываем score
            
            previousDistance = -1; // Сбрасываем previousDistance
            isStopped = false; // Сбрасываем isStopped
        }
        private float CalculateWallPenalty(float[] sensorDistances)
        {
            const float minSafeDistance = 0.15f; // 15% от максимального расстояния
            const float warningDistance = 0.25f;  // 25% от максимального расстояния

            float minDistance = sensorDistances.Min();
            float penalty = 0f;

            // Только серьезные нарушения безопасности получают штраф
            if (minDistance < minSafeDistance)
            {
                // Мягкий штраф - только при реальной опасности
                penalty = (minSafeDistance - minDistance) * 5f;
            }
            else if (minDistance < warningDistance)
            {
                // Очень мягкий штраф в зоне предупреждения
                penalty = (warningDistance - minDistance) * 1f;
            }

            return -penalty;
        }
        private void SerializeCommands(Dictionary<string, int> pairs)
        {
            jsonString = JsonConvert.SerializeObject(pairs, Formatting.None);
        }
        private float CalculateReward(float currentDistance, float timeSpent, float deltaTime, float[] sensorDistances)
        {
            float distanceDelta = previousDistance - currentDistance;
            float speed = distanceDelta / deltaTime;
            float reward = 0f;

            // Щедрое вознаграждение за движение к цели
            if (speed > 0.01f) // Движение вперед
            {
                // Базовое вознаграждение + бонус за скорость
                reward += 5f + speed * 20f;

                // Дополнительный бонус за движение в безопасной зоне
                if (sensorDistances.Min() > 0.3f)
                {
                    reward += 2f;
                }
            }
            else if (speed < -0.01f) // Движение назад
            {
                // Мягкий штраф за движение в неправильном направлении
                reward += speed * 10f;
            }
            else // Стоим на месте
            {
                // Небольшой штраф за бездействие
                reward -= 0.5f * deltaTime;
            }

            // Бонус за приближение к цели
            if (currentDistance < previousDistance)
            {
                reward += 1f;
            }

            // Очень мягкий штраф за время (уменьшен в 10 раз)
            reward -= 0.01f * deltaTime;

            // Штраф за стены (только при реальной опасности)
            reward += CalculateWallPenalty(sensorDistances);

            // Большой бонус за достижение цели!
            if (currentDistance < 0.5f) // 0.5 метра до цели
            {
                reward += 100f;
                InformTextbox.Text += "\r\n[SUCCESS] Цель достигнута! Большой бонус!";
            }

            return reward;
        }

        private float Distance(float x1, float y1, float x2, float y2)
        {
            // Добавим небольшую константу, чтобы избежать деления на ноль
            return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)) + 0.001f;
        }
        //// Модифицируйте метод CalculateReward
        //private float CalculateReward(float currentDistance, float timeSpent, float deltaTime, float[] sensorDistances)
        //{
        //    float distanceDelta = previousDistance - currentDistance;
        //    float speed = distanceDelta / deltaTime;

        //    float reward = 0f;
        //    bool isMoving = Math.Abs(speed) > 0.01f;

        //    // Базовые награды/штрафы
        //    if (speed > 0.01f)
        //    {
        //        // Награда за движение к цели уменьшается при близости к стенам
        //        float proximityFactor = 1f - (0.2f - sensorDistances.Min());
        //        reward += speed * 10 * Math.Max(0.1f, proximityFactor);
        //    }
        //    else if (speed < -0.01f)
        //    {
        //        reward += speed * 20;
        //    }
        //    else
        //    {
        //        reward -= 5f * deltaTime;
        //    }

        //    // Штраф за время
        //    reward -= timePenaltyFactor * deltaTime;

        //    // Штраф за близость к стенам
        //    float wallPenalty = CalculateWallPenalty(sensorDistances);
        //    reward -= wallPenalty;

        //    return reward;
        //}
        //private float CalculateWallPenalty(float[] sensorDistances)
        //{
        //    const float safeDistance = 30; // 30 см (1500mm * 0.2 = 300mm)
        //    float minDistance = sensorDistances.Min();

        //    // Если все датчики показывают безопасное расстояние - поощрение
        //    if (minDistance >= safeDistance)
        //    {
        //        // Поощрение за поддержание безопасной дистанции
        //        return 2f; // Бонусные очки за движение в безопасной зоне
        //    }

        //    // Штраф только если приблизились ближе 30 см
        //    float penalty = 0f;
        //    if (minDistance < safeDistance)
        //    {
        //        // Линейный штраф: чем ближе к стене, тем сильнее штраф
        //        penalty = (safeDistance - minDistance) * 20f;

        //        // Дополнительный штраф за боковые датчики
        //        if (sensorDistances[0] < safeDistance || sensorDistances[7] < safeDistance)
        //        {
        //            penalty += 5f; // Дополнительный штраф за опасное боковое сближение
        //        }
        //    }

        //    return -penalty; // Возвращаем отрицательное значение как штраф
        //}

        //private float CalculateReward(float currentDistance, float timeSpent, float deltaTime, float[] sensorDistances)
        //{
        //    float distanceDelta = previousDistance - currentDistance;
        //    float speed = distanceDelta / deltaTime;

        //    float reward = 0f;
        //    bool isMovingForward = speed > 0.01f;
        //    bool isMovingAway = speed < -0.01f;

        //    // Базовые награды за движение
        //    if (isMovingForward)
        //    {
        //        reward += speed * 15; // Увеличенная награда за движение к цели
        //    }
        //    else if (isMovingAway)
        //    {
        //        reward += speed * 25; // Увеличенный штраф за движение от цели
        //    }
        //    else
        //    {
        //        reward -= 3f * deltaTime; // Штраф за простой
        //    }

        //    // Учет расстояния до стен
        //    float wallEffect = CalculateWallPenalty(sensorDistances);
        //    reward += wallEffect;

        //    // Дополнительная награда за движение в безопасной зоне
        //    if (sensorDistances.Min() > 0.2f && isMovingForward)
        //    {
        //        reward += 1f * deltaTime; // Небольшой постоянный бонус
        //    }

        //    // Штраф за время (чтобы стимулировать быстрее находить выход)
        //    reward -= timePenaltyFactor * deltaTime;

        //    return reward;
        //}

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ipAddress = IPAddress.Parse(textBoxIP.Text);
            port = Int32.Parse(textBoxLocalPort.Text);

            iPEndPoint = new IPEndPoint(ipAddress, port);
            udpClient = new UdpClient(port);
            thread = new Thread(() => Receive());
            thread.Start();
            Thread.Sleep(1000);
            timer1.Enabled = true;
            timer1.Start();
            startTime = Environment.TickCount / 1000f;
            
        }


        private async void timer1_Tick(object sender, EventArgs e)
        {
            if (data != null)
            {
                DecodingData(data);
                

                // Отправка по UDP
                Send();
            }
            else
            {
                textBoxData.Text += "\r\n[ERROR] No data";
            }
        }

        private int DetermineCorrectAction(float[] inputs)
        {
            // Если робот близко к стенке слева
            if (inputs[0] > 0.7f || inputs[7] > 0.7f)
            {
                return 2; // RIGHT
            }
            // Если робот близко к стенке справа
            else if (inputs[3] > 0.7f || inputs[4] > 0.7f)
            {
                return 0; // LEFT
            }
            // Если робот находится в центре коридора
            else if (inputs[1] > 0.5f && inputs[2] > 0.5f && inputs[5] > 0.5f && inputs[6] > 0.5f)
            {
                return 1; // FORWARD
            }
            // Если робот не может двигаться вперед
            else if (inputs[1] < 0.3f && inputs[2] < 0.3f && inputs[5] < 0.3f && inputs[6] < 0.3f)
            {
                return 0; // LEFT
            }
            else
            {
                // Если робот не может двигаться влево или вправо, то он должен двигаться вперед
                return 1; // FORWARD
            }
        }


        private int[,] LoadMapToArray(string fileName)
        {
            string[] lines = System.IO.File.ReadAllLines(fileName);
            int rows = lines.Length;
            int cols = lines[0].Length;

            int[,] mapArray = new int[rows, cols];

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    mapArray[y, x] = (lines[y][x] == '#') ? 0 : 1;
                }
            }

            return mapArray;
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
                timer1.Stop();
            }
            else
            {
                timer1.Enabled = true;
                timer1.Start();
            }
        }

        private void ShowWeightsInGrid(bool showInputToHidden)
        {
            dataGridWeights.Columns.Clear();
            dataGridWeights.Rows.Clear();

            if (showInputToHidden)
            {
                for (int j = 0; j < nn.hiddenSize; j++)
                {
                    dataGridWeights.Columns.Add("H" + j, "H" + j);
                }

                for (int i = 0; i < nn.inputSize; i++)
                {
                    var row = new DataGridViewRow();
                    row.CreateCells(dataGridWeights);

                    for (int j = 0; j < nn.hiddenSize; j++)
                    {
                        row.Cells[j].Value = nn.weightsInputHidden[i, j];
                    }

                    dataGridWeights.Rows.Add(row);
                }
            }
            else
            {
                for (int j = 0; j < nn.outputSize; j++)
                {
                    dataGridWeights.Columns.Add("O" + j, "O" + j);
                }

                for (int i = 0; i < nn.hiddenSize; i++)
                {
                    var row = new DataGridViewRow();
                    row.CreateCells(dataGridWeights);

                    for (int j = 0; j < nn.outputSize; j++)
                    {
                        row.Cells[j].Value = nn.weightsHiddenOutput[i, j];
                    }

                    dataGridWeights.Rows.Add(row);
                }
            }
        }

        private void ApplyWeightsFromGrid(bool applyToInputHidden)
        {
            if (applyToInputHidden)
            {
                for (int i = 0; i < nn.inputSize; i++)
                {
                    for (int j = 0; j < nn.hiddenSize; j++)
                    {
                        nn.weightsInputHidden[i, j] = float.Parse(dataGridWeights.Rows[i].Cells[j].Value.ToString());
                    }
                }
            }
            else
            {
                for (int i = 0; i < nn.hiddenSize; i++)
                {
                    for (int j = 0; j < nn.outputSize; j++)
                    {
                        nn.weightsHiddenOutput[i, j] = float.Parse(dataGridWeights.Rows[i].Cells[j].Value.ToString());
                    }
                }
            }
        }

        private void btnShowWeights_Click(object sender, EventArgs e)
        {
            bool showInputHidden = radioInputHidden.Checked; // или comboBox.SelectedItem
            ShowWeightsInGrid(showInputHidden);
        }

        private void btnApplyWeightsFromGrid_Click(object sender, EventArgs e)
        {
            bool toInputHidden = radioInputHidden.Checked;
            ApplyWeightsFromGrid(toInputHidden);
        }

        private void btnSaveWeightsToFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files|*.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                nn.SaveWeights(sfd.FileName);
            }
        }

        private void btnLoadWeightsFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files|*.txt";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                nn.LoadWeights(ofd.FileName);
            }
        }
        public class Robot
        {
            public static void Moving(string command)
            {
                message = "Current command is" + command;
                int B = 0, F = 0;

                if (command == "FORWARD")
                {
                    F = 100;
                    B = 0;
                }

                else if (command == "LEFT")
                {
                    F = 10;
                    B = -100;
                }
                else if (command == "RIGHT")
                {
                    F = 10;
                    B = 100;
                }
                else
                {
                    F = 0;
                    B = 0;
                }
                commands["B"] = B;
                commands["F"] = F;
                commands["N"] += 1;
            }
        }

    }
}