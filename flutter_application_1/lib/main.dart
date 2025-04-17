//import 'package:english_words/english_words.dart';
import 'package:flutter/material.dart';
//import 'package:provider/provider.dart';
import 'high_school_page.dart';


void main() => runApp(MaterialApp(
  home: Home()

));
class Home extends StatelessWidget { //Base page design, Home buttons
  const Home({super.key});
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Power Outage Education App', style: TextStyle(fontSize: 30, color: Colors.white)),
        centerTitle: true,
        backgroundColor: Colors.blue,
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: <Widget>[
          Spacer(),
          Flexible(
            flex: 5,
            fit: FlexFit.loose,
            child: Center(
              child: Material(
                color: Colors.blue,
                elevation: 8,
                borderRadius: BorderRadius.circular(28),
                clipBehavior: Clip.antiAliasWithSaveLayer,
                child: InkWell(
                  onTap: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => const ElementaryPage()),
                    );
                  },
                  child: Column(
                    children: [
                      Ink.image(
                        image: AssetImage('assets/placeholder.jpg'),
                        height: 170,
                        width: 300,
                        fit: BoxFit.cover,
                      ),
                      SizedBox(height: 6),
                      Text('K-5 Elementary',style: TextStyle(fontSize: 20, color: Colors.white)),
                    ],
                  ),
                ),
              ),
            ),
           ),
          Spacer(),
          Flexible(
            flex: 5,
            child: Center(
              child: Material(
                color: Colors.blue,
                elevation: 8,
                borderRadius: BorderRadius.circular(28),
                clipBehavior: Clip.antiAliasWithSaveLayer,
                child: InkWell(
                  onTap: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => const MiddleSchoolPage()),
                    );
                  },
                  child: Column(
                    children: [
                      Ink.image(
                        image: AssetImage('assets/placeholder.jpg'),
                        height: 170,
                        width: 300,
                        fit: BoxFit.cover,
                      ),
                      SizedBox(height: 6),
                      Text('6-8th Middle School',style: TextStyle(fontSize: 20, color: Colors.white)),
                    ],
                  ),
                ),
              ),
            ),
           ),
          Spacer(),
          Flexible(
            flex: 5,
            child: Center(
              child: Material(
                color: Colors.blue,
                elevation: 8,
                borderRadius: BorderRadius.circular(28),
                clipBehavior: Clip.antiAliasWithSaveLayer,
                child: InkWell(
                  onTap: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => const HighSchoolPage()),
                    );
                  },
                  child: Column(
                    children: [
                      Ink.image(
                        image: AssetImage('assets/placeholder.jpg'),
                        height: 170,
                        width: 300,
                        fit: BoxFit.cover,
                      ),
                      SizedBox(height: 6),
                      Text('9-12th High School',style: TextStyle(fontSize: 20, color: Colors.white)),
                    ],
                  ),
                ),
              ),
            ),
          ),
          Spacer(),
        ]
      )
    );
  }
}

class ElementaryPage extends StatelessWidget {
  const ElementaryPage({super.key});
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('K-5 Elementary Page', style: TextStyle(fontSize: 30, color: Colors.white)),
        centerTitle: true,
        backgroundColor: Colors.blue,
      ),
      body: Center(
        child: ElevatedButton(onPressed: (){
          Navigator.pop(context);
        },
            child: const Text('Return to Home Page')),
      ),
    );
  }
}

class MiddleSchoolPage extends StatelessWidget {
  const MiddleSchoolPage({super.key});
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('6-8th Middle School Page', style: TextStyle(fontSize: 30, color: Colors.white)),
        centerTitle: true,
        backgroundColor: Colors.blue,
      ),
      body: Center(
        child: ElevatedButton(onPressed: (){
          Navigator.pop(context);
        },
            child: const Text('Return to Home Page')),
      ),
    );
  }
}
