   M S B A M L   `   `   `  ����S  Owykrywanie otworkow_test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:wykrywanie_otworkow_testwykrywanie_otworkow_test  5   	   )    #wykrywanie_otworkow_test.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:wykrywanie_otworkow_test     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$ 
MainWindow��5	   	   $	��750��6   $
��1200��6)   ��5
      �� 5   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �  � Color$ White��6    � Offset$ 0.478Q�6)   � 5      $ 	#FFE6F0F9��6   $ 0.688Q�6-   � 5      $ 	#FFF8F1FA��6   $ 1Q�6-   5      .��5      �  � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $��3* �6   �� 5      $
��0.3* �6    � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   �� -   5         ��Name$ button��5      $��Button��6!   $��	468,0,0,0q�62   $	��132��6E   $��2��6f   $
��Left=�6s   $y�Hidden=�6�   �� 5   
   -   5        	 ��Name$	 image_capture��5      $��1,1,1,1q�6'   $��1��6`   $��1��6q   �� 5   
   -   5        $ btn_parametry��5      $��	Parametry��6(   $��2��6U   $
��Left=�6b   $��Center=�6}   $��51��6�   $	��105��6�   $��-2,0,0,0q�6�   $��16(�6�   ��5      �� 5      $ 0.5,1&�6&   $ 0.5,0&�67   .��5       � $ 	#FFACCBE7��6#   $ 1Q�65   � 5!      $ 	#FF89C2E5��6#   $ 0.654Q�65   � 5"      $ 	#FFE8F2F7��6#   $ 0Q�65   5#      5&      �� 5'   
   -   5        $	 image_stream��5'      $��1,1,1,1q�6&   $��1��67   $��1��6K   �� 5(   
   -   5        
 ��Name$
 Podaj_barkode��5(      $��Zeskanuj numer seryjny��6'   $��0��6H   $	��Top=�6U   $
��Left=�6m   $��	34,26,0,0q�6�   $	��342��6�   $��24(�6�   �� 5)   
   -   5         ��Name$ textBox��5)      $��32,0,128,20q�6#   $�� ��68    �� TextWrapping$
 Wrap=�6@   $��0��6|   $��Bottom=�6�   $��41��6�   $��20(�6�   �� 5*   
   -   5        $
 label_state��5*      $��1.Zeskanuj produkt��6%   $��1��6B   $��Center=�6R   $��Center=�6o   $��24(�6�   $	��500��6�   $	��120��6�   �� 5+   
   -   5        $
 label��5+      $��Timer��6   $��1��6/   $��Center=�6?   $��0,0,0,0q�6\   $��2��6m   $��Center=�6z   $y�Hidden=�6�   �� 5,   
   -	   5        $
 l_n_wykryto_lewa��5,      $��Wykryto lewą zaślepkę��6*   $��1��6J   $
��Left=�6Z   $��2��6u   $��Center=�6�   $��18(�6�   ���	#FF86EE30$y�Hidden=�6�   �� 5-   
   -
   5        $
 l_n_wykryto_prawa��5-      $��Wykryto prawą zaślepkę��6+   $��1��6L   $��2��6\   $��Center=�6i   $��18(�6�   $��0,0,5,0q�6�   $��Right=�6�   ���	#FF86EE30$y�Hidden=�6�   50      51      