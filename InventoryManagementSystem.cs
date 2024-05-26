using CRM.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;



namespace CRM.Library.Services
{
    public class InventoryManagementSystem
    {

        private InventoryManagementSystem() {
            contacts = new List<package>();
        }
        private List<package> inventory = new List<package>();
        private List<package> cart = new List<package>();


        private int nextId = 1;


        private static InventoryManagementSystem? instance;
        private static object instanceLock = new object();
        public static InventoryManagementSystem Current
        {
            get
            {
                lock(instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new InventoryManagementSystem();
                    }
                }

                return instance;
            }
        }

        private List<package>? contacts;
        public ReadOnlyCollection<package>? Contacts
        {
            get
            {
                return contacts?.AsReadOnly();
            }
        }

        //======== functionality
        public void Start()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Inventory Management");
                Console.WriteLine("2. Shop");
                Console.WriteLine("3. Exit");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageInventory();
                        break;
                    case "2":
                         Shop();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void ManageInventory()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Inventory Management");
                Console.WriteLine("1. Create Item");
                Console.WriteLine("2. Read Items");
                Console.WriteLine("3. Update Item");
                Console.WriteLine("4. Delete Item");
                Console.WriteLine("5. Back to Main Menu");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateItem();
                        break;
                    case "2":
                        ReadItems();
                        break;
                    case "3":
                  
                        break;
                    case "4":
                        DeleteItem();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void Shop()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Shop");
                Console.WriteLine("1. View Inventory");
                Console.WriteLine("2. Add Item to Cart");
                Console.WriteLine("3. Remove Item from Cart");
                Console.WriteLine("4. View Cart");
                Console.WriteLine("5. Checkout");
                Console.WriteLine("6. Back to Main Menu");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ReadItems();
                        break;
                    case "2":
                        AddItemToCart();
                        break;
                    case "3":
                        RemoveItemFromCart();
                        break;
                    case "4":
                        ViewCart();
                        break;
                    case "5":
                        Checkout();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void CreateItem()
        {
            Console.Clear();
            var item = new package { Id = nextId++ };

            Console.Write("Enter name: ");
            item.Name = Console.ReadLine();
            Console.Write("Enter description: ");
            item.Description = Console.ReadLine();
            Console.Write("Enter price: ");
            item.Price = decimal.Parse(Console.ReadLine());
            Console.Write("Enter quantity: ");
            item.Quantity = int.Parse(Console.ReadLine());

            inventory.Add(item);
            Console.WriteLine("Item created successfully!");
            Console.ReadLine();
        }

        private void ReadItems()
        {
            Console.Clear();
            foreach (var item in inventory)
            {
                Console.WriteLine($"ID: {item.Id}");
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Description: {item.Description}");
                Console.WriteLine($"Price: {item.Price:C}");
                Console.WriteLine($"Quantity: {item.Quantity}");
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        private void DeleteItem()
        {
            Console.Clear();
            Console.Write("Enter ID of the item to delete: ");
            var id = int.Parse(Console.ReadLine());

            var item = inventory.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                Console.WriteLine("Item not found!");
                Console.ReadLine();
                return;
            }

            inventory.Remove(item);
            Console.WriteLine("Item deleted successfully!");
            Console.ReadLine();
        }

        private void AddItemToCart()
        {
            Console.Clear();
            Console.Write("Enter ID of the item to add to cart: ");
            var id = int.Parse(Console.ReadLine());

            var item = inventory.FirstOrDefault(i => i.Id == id);
            if (item == null || item.Quantity <= 0)
            {
                Console.WriteLine("Item not available!");
                Console.ReadLine();
                return;
            }

            var cartItem = cart.FirstOrDefault(i => i.Id == id);
            if (cartItem == null)
            {
                cartItem = new package
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Quantity = 0
                };
                cart.Add(cartItem);
            }

            Console.Write("Enter quantity to add: ");
            var quantity = int.Parse(Console.ReadLine());
            if (quantity > item.Quantity)
            {
                Console.WriteLine("Not enough items in stock!");
                Console.ReadLine();
                return;
            }

            item.Quantity -= quantity;
            cartItem.Quantity += quantity;

            Console.WriteLine("Item added to cart!");
            Console.ReadLine();
        }

        private void RemoveItemFromCart()
        {
            Console.Clear();
            Console.Write("Enter ID of the item to remove from cart: ");
            var id = int.Parse(Console.ReadLine());

            var cartItem = cart.FirstOrDefault(i => i.Id == id);
            if (cartItem == null)
            {
                Console.WriteLine("Item not in cart!");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter quantity to remove: ");
            var quantity = int.Parse(Console.ReadLine());
            if (quantity > cartItem.Quantity)
            {
                Console.WriteLine("You can't remove more than what's in the cart!");
                Console.ReadLine();
                return;
            }

            cartItem.Quantity -= quantity;
            if (cartItem.Quantity == 0)
            {
                cart.Remove(cartItem);
            }

            var inventoryItem = inventory.First(i => i.Id == id);
            inventoryItem.Quantity += quantity;

            Console.WriteLine("Item removed from cart!");
            Console.ReadLine();
        }

        private void ViewCart()
        {
            Console.Clear();
            foreach (var item in cart)
            {
                Console.WriteLine($"ID: {item.Id}");
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Description: {item.Description}");
                Console.WriteLine($"Price: {item.Price:C}");
                Console.WriteLine($"Quantity: {item.Quantity}");
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        private void Checkout()
        {
            Console.Clear();
            decimal subtotal = 0;
            foreach (var item in cart)
            {
                Console.WriteLine($"ID: {item.Id}");
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Description: {item.Description}");
                Console.WriteLine($"Price: {item.Price:C}");
                Console.WriteLine($"Quantity: {item.Quantity}");
                var totalItemPrice = item.Price * item.Quantity;
                Console.WriteLine($"Total: {totalItemPrice:C}");
                Console.WriteLine();
                subtotal += (Decimal)totalItemPrice;

            }

            decimal tax = subtotal * 0.07m;
            decimal total = subtotal + tax;

            Console.WriteLine($"Subtotal: {subtotal:C}");
            Console.WriteLine($"Tax (7%): {tax:C}");
            Console.WriteLine($"Total: {total:C}");

            Console.WriteLine("Press any key to complete checkout...");
            Console.ReadLine();

            cart.Clear();
            Console.WriteLine("Checkout complete!");
            Console.ReadLine();
        }
    }





}

