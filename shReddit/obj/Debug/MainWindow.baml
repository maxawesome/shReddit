   M S B A M L   `   `   `  ����  shReddit    shReddit.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  shReddit��5   	   $��638.73��6   $��462.001��6*    � Icon$ snoo.ico��5   	    � 
ResizeMode$ CanMinimize=�6    � SizeToContent$ WidthAndHeight=�62   .��5      � .+�5   
   ]� .+�5      �� $
��Left=�6   $	��450��6/   $	��600��6;   .+�5	      ]� $	��Top=�6   $��Center=�64   $��20,20,20,20q�6Q   .+�5
      �� 5      �� 5      .+�5      �� -   5         ��Name$ WelcomeLabel��5       $	��Top=�66   $��Center=�6K   $
��Bold"�6h   $��24(�6z   $��Welcome to shReddit!��6�   $��43��6�   $	��265��6�   �� 5      -   5         ��Name$ 	LogoImage��5       $��Images/shreddit_off.png��64   $	��148��6U   $	��180��6b   $��Bottom=�6n   $��Center=�6�   5      5      ]� 5      $��Center=�6   $��Center=�67   .+�5      �� $
��Left=�6!   .+�5      �� -   5        $ UserNameLabel��5       $��Center=�67   $��Right=�6R   $��	Username:��6n   $��65��6�   �� 5      -   5        $ PasswordLabel��5       $��Center=�67   $��Right=�6R   $��	Password:��6n   $��65��6�   5      �� 5      $
��Left=�6!   $	��211��67   .+�5      �� -   5         ��Name$ UserNameText��5   "   ��. $��0��6I   $��Center=�6V   $
��Left=�6q   $��5,5,0,5q�6�   $	��196��6�   
�������6�   G� 5      -   5         G�Name$ PasswordText��5   &   $��Center=�6<   $
��Left=�6W   $��5,5,0,5q�6r   $	��196��6�   �� 5      -   5         ��Name$ LoginButton��5   !   $��Bottom=�66   $��Center=�6N   $��Center=�6i   $��5,2,5,2q�6�   $��5,2,10.111,2q�6�   $��Login��6�   $	��196��6�   ���	#FF29C4076�   5      5      ]� 5      -   5        $��OptionsDock��5      $��Center=�6/   $��Center=�6J   $y�	Collapsed=�6g   .+�5      �� $
��Left=�6!   $
��Left=�67   $	��167��6R   .+�5      �� -	   5        $ WriteGarbageLabel��5       $��Center=�6;   $��Right=�6V   $��Write Garbage?��6r   �� 5      -
   5        $ PassNumberLabel��5       $��Center=�69   $��Right=�6T   $��Number of Passes:��6p   �� 5       -   5        $ DeletePostsLabel��5        $��Center=�6:   $��Right=�6U   $��# of Posts to Shred:��6q   �� 5!      -   5        $ DeleteCommentsLabel��5!       $��Center=�6=   $��Right=�6X   $��# of Comments to Shred:��6t   5"      �� 5#      $��Right=�6!   $��Right=�68   $	��110��6T   .+�5$      �� -   5        	 ��Name$	 WriteGarbage��5$   #   
 � ToolTip$L
 FWrites a random string of text to each post/comment prior to deletion.��69   $��Center=�6�   $
��Left=�6�   $��5,1,0,3q�6�   $��50��6�   $	��100��6�   $	��103��6�   .@�5%      ��  �� 
IsSelected . $��No��6=   �� 5&      $	��Yes��6+   5'      �� 5(      -   5        $	 
PassNumber��5(   #   $a
 [Determines the number of times to write random text to each post/comment prior to deletion.��67   $��Center=�6�   $
��Left=�6�   $��5,1,0,3q�6�   $��50��6�   $	��100��6�   $	��103��6  .@�5)      ��  . $��1��6=   �� 5*      $��3��6+   �� 5+      $��5��6+   �� 5,      $��10��6+   5-      �� 5.      -   5        $	 DeletePostsQuantity��5.   #   $S
 MSpecifies the number of posts to shred. Max is 1000 due to Reddit API limits.��6@   $��Center=�6�   $
��Left=�6�   $��5,1,0,3q�6�   $��50��6�   $	��100��6�   $	��103��6�   .@�5/      ��  . $��0��6=   50      �� 51      -   5        $	 DeleteCommentsQuantity��51   #   $V
 PSpecifies the number of comments to shred. Max is 1000 due to Reddit API limits.��6C   $��Center=�6�   $
��Left=�6�   $��5,1,0,3q�6�   $��50��6�   $	��100��6�   $	��103��6  .@�52      ��  . $��0��6=   53      54      55      ]� 56      -   5        $��	ShredDock��56      $��Center=�6-   $��Center=�6H   $y�	Collapsed=�6e   .+�57      �� $
��Left=�6!   $��Right=�67   $	��265��6S   .+�58      �� -   5        $ ShredButton��58   !   $��Bottom=�66   $��Center=�6N   $��Right=�6i   $��5,2,5,2q�6�   $	��200��6�   $	��350��6�   $	��0,2q�6�   {�.  $��Shred��6�   $	��200��6�   ���	#FFA40E0E6
  59      5:      �� 5;      -   5        $��
OutputDock��5;      $��Bottom=�6/   $y�	Collapsed=�6G   .+�5<      �� ��. $��Bottom=�6;   $��5,5,5,5q�6V   $��95��6g   $	��379��6s   $��1q�6   #��� ��6�   .��5=      �� -   5         ��Name$ OutputTextBlock��5=   $   $��Bottom=�6=   $
��Left=�6X   $��10,10,10,10q�6s    �� TextAlignment$
 Left=�6�   $
��Wrap=�6�   �� 6�   $<�6Once you start shredding, you'll see the results here.��6�   6�   5>      5?      5@      5A      5B      5C      