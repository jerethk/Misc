package skillage1;

import java.io.*;
import java.sql.*;
import java.util.ArrayList;

public class Main {
	private static final String connectionString = "jdbc:sqlite:d:/jereth/TTFProds.db";  
	
	public static void main(String[] args) {
		//System.out.print("hello world");
		
		ArrayList<Product> productList = new ArrayList<>();
		String CSVPath = "C:\\Users\\Jereth\\Documents\\Documents\\Study\\Upskilled\\18 Mob\\Project\\Docs\\TTF_Products.csv";
		
		if (readCSV(CSVPath, productList)) {
			System.out.println(productList.size() + " products in list.");
			createDB(productList);
		}
	}
	
	private static boolean readCSV(String path, ArrayList<Product> productList) {
		String nextLine;
		String[] splitLine;
		
		try (BufferedReader reader = new BufferedReader(new FileReader(path))) {
			System.out.println(reader.readLine());
			
			while ((nextLine = reader.readLine()) != null) {
				splitLine = nextLine.split(",");
				
				Product p = new Product();
				p.setItemCode(splitLine[0]);
				p.setItemDescription(splitLine[1]);
				p.setItemCount(Integer.parseInt(splitLine[2]));
				
				productList.add(p);
			}
			
		} catch (IOException e) {
			System.out.println(e.toString());
			System.out.println("Error reading file.");
			return false;
		} 
		
		return true;
	}

	private static boolean createDB(ArrayList<Product> productList) {
		final String CREATE_TABLE = "CREATE TABLE Product_stocks (item_code VARCHAR PRIMARY KEY, item_description VARCHAR, current_count INTEGER)";
		
		try (Connection conn = DriverManager.getConnection(connectionString)) {
			Statement statement = conn.createStatement();
			//statement.execute("DROP TABLE Product_stocks");
			statement.execute(CREATE_TABLE);
			
			for (Product p : productList) {
				String insertStatement = "INSERT INTO Product_stocks VALUES ('" + p.getItemCode() + "','" + p.getItemDescription() + "'," + p.getItemCount() + ")"; 
				//System.out.println(insertStatement);
				
				statement.execute(insertStatement);
			}
			
		} catch (SQLException e) {
			System.out.println(e.toString());
			return false;
		}
		
		System.out.println("Done");
		return true;
	}
}
